using System.Text.Json;
using System.Text.Json.Nodes;

using ApiQL.Language;

using SqlKata;
using SqlKata.Execution;

namespace ApiQL;

public class ApiQueryLanguage : AbstractLanguage
{
    public readonly string[] ALLOWED_FUNCTIONS = ["sum", "count", "min", "max", "avg", "first", "last"];
    public readonly string[] SPECIAL_FUNCTIONS = ["first", "last"];

    // private const string Logic = "and";
    // private const string REL = "eq";

    /// <summary>
    /// Query builder implementation
    /// </summary>
    // private readonly Language.ApiQueryBuilder builder;
    
    // private Query _builder;
    private Dictionary<string, Type> _specs;
    private JsonObject _expression;
    

    // public ApiQueryLanguage(JsonObject expression, Query builder, string[]? specs = default) : base(expression)
    // {
    //     FilterExpression(ref _expression);
    //     _builder = new ApiQueryBuilder(builder);
    // }
    
    public ApiQueryLanguage(JsonObject expression, Query builder, Dictionary<string, Type> specs = null)
    {
        // Фильтрация выражения
        FilterExpression(ref expression);
        _expression = expression; // Список для значений выражения
        
        _builder = new ApiQueryBuilder(builder);
        
        // Инициализация спецификаций
        _specs = new Dictionary<string, Type>
        {
            // { "Equals", typeof(Equals) },
            // { "NotEquals", typeof(NotEquals) },
            // { "IsNull", typeof(IsNull) },
            // { "LessThan", typeof(LessThan) },
            // { "GreaterThan", typeof(GreaterThan) },
            // { "Between", typeof(Between) },
            // { "IsNotNull", typeof(IsNotNull) },
            // { "LessThanOrEqual", typeof(LessThanOrEqual) },
            // { "GreaterThanOrEqual", typeof(GreaterThanOrEqual) },
            // { "Like", typeof(Like) },
            // { "NotLike", typeof(NotLike) },
            // { "ContainsString", typeof(ContainsString) },
            // { "NotContainsString", typeof(NotContainsString) },
            // { "StartsWith", typeof(StartsWith) },
            // { "EndsWith", typeof(EndsWith) },
            // { "In", typeof(In) },
            // { "NotIn", typeof(NotIn) },
            // { "EqualsAny", typeof(EqualsAny) },
            // { "NotEqualsAny", typeof(NotEqualsAny) },
            // { "EqualsAll", typeof(EqualsAll) },
            // { "NotEqualsAll", typeof(NotEqualsAll) },
            // { "Address", typeof(Address) },
            // { "GarAddress", typeof(GarAddress) },
            // { "Search", typeof(Search) }
        };
    }

    /// <summary>
    /// Execute specification root expression
    /// </summary>
    /// <returns></returns>
    public override object? Execute()
    {
        ExecuteFunctions();
        ExecuteGroup();
        ExecuteOffset();
        ExecuteLimit();
        ExecuteOrder();
        ExecuteFields();
        ExecuteWhere();
        return null;
    }

    private static void FilterExpression(ref JsonObject expression)
    {
        expression.Remove("_url");
    }

    /// <summary>
    /// Execute function fields of expression
    /// </summary>
    private void ExecuteFunctions()
    {
        var keysToRemove = new List<string>();

        foreach (var property in _expression)
        {
            if (property.Value is null) continue;

            var key = property.Key;
            var value = property.Value;
            if (value is not JsonObject jsonObject) continue;

            if (jsonObject.ContainsKey("func"))
            {
                var func = jsonObject["func"]?.GetValue<string>();
                if (func is not null && ALLOWED_FUNCTIONS.Contains(func))
                {
                    if (SPECIAL_FUNCTIONS.Contains(func))
                    {
                        var field = SpecialFunctionsBuilder.Execute(key, jsonObject);
                        if (!string.IsNullOrEmpty(field))
                        {
                            _builder.AddSelect(field);
                        }
                    }
                    else
                    {
                        var field = key == "*" ? "(*)" : $"(\"{key}\")";
                        if (jsonObject.ContainsKey("as"))
                        {
                            field += $" as \"{jsonObject["as"].GetValue<string>()}\"";
                        }

                        _builder.AddSelect($"{func}{field}");
                    }
                }

                keysToRemove.Add(key);
            }
            else if (jsonObject.ContainsKey("as"))
            {
                _builder.AddSelect($"\"{key}\" as \"{jsonObject["as"]!.GetValue<string>()}\"");
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            _expression.Remove(key);
        }
    }

    private void ExecuteGroup()
    {
        if (!_expression.TryGetPropertyValue("group", out var groupNode) || groupNode is null) return;

        string[] fields;
        if (groupNode is JsonArray groupArray)
        {
            fields = groupArray.Where(x => x is not null).Select(node => $"\"{node!.GetValue<string>()}\"").ToArray();
        }
        else
        {
            fields = [$"\"{groupNode.GetValue<string>()}\""];
        }

        _builder.AddGroupBy(fields);

        _expression.Remove("group");
    }

    private void ExecuteOffset()
    {
        if (!_expression.TryGetPropertyValue("offset", out var offsetNode) || offsetNode is null) return;

        _builder.SetFirstResult(offsetNode.GetValue<int>());
        _expression.Remove("offset");
    }

    private void ExecuteLimit()
    {
        if (!_expression.TryGetPropertyValue("limit", out var limitNode) || limitNode is null) return;

        _builder.SetMaxResults(limitNode.GetValue<int>());
        _expression.Remove("limit");
    }

    private void ExecuteOrder()
    {
        if (!_expression.TryGetPropertyValue("order", out var orderNode) || orderNode is null) return;

        string[] fields;
        if (orderNode is JsonArray orderArray)
        {
            fields = orderArray.Where(x => x is not null).Select(node => node!.GetValue<string>()).ToArray();
        }
        else
        {
            fields = orderNode.GetValue<string>().Split(',');
        }

        if (fields.Length > 0)
        {
            foreach (var field in fields)
            {
                var pair = field.Split(':');
                var fieldName = pair[0];
                var orderType = pair.Length == 2 ? pair[1] : null;
                _builder.AddOrderBy(fieldName, orderType);
            }
        }

        _expression.Remove("order");
    }

    private void ExecuteFields()
    {
        var isProjection = false;
        var isDistinct = false;
        string? fieldsName = null;

        if (_expression.ContainsKey("fields"))
        {
            isProjection = true;
            fieldsName = "fields";
        }
        else if (_expression.ContainsKey("select"))
        {
            isProjection = true;
            fieldsName = "select";
        }

        if (_expression.TryGetPropertyValue("distinct", out var distinctNode))
        {
            isDistinct = distinctNode?.GetValue<string>().Equals("true", StringComparison.OrdinalIgnoreCase) == true ||
                         distinctNode?.GetValue<bool>() == true;
            _expression.Remove("distinct");
        }

        if (isProjection)
        {
            string[] fields;
            if (_expression[fieldsName!] is JsonArray fieldArray)
            {
                fields = fieldArray.Where(x => x is not null).Select(f => f!.GetValue<string>()).ToArray();
            }
            else
            {
                fields = _expression[fieldsName!]!.GetValue<string>().Split(',');
            }

            if (fields.Length > 0)
            {
                for (var i = 0; i < fields.Length; i++)
                {
                    if (isDistinct && i == 0)
                    {
                        _builder.AddSelect($"distinct \"{fields[i]}\"");
                    }
                    else
                    {
                        _builder.AddSelect($"\"{fields[i]}\"");
                    }
                }
            }
            else if (_builder.GetQueryPart("select").Count == 0)
            {
                _builder.AddSelect(isDistinct ? "distinct *" : "*");
            }

            _expression.Remove(fieldsName!);
        }
        else if (_builder.GetQueryPart("select").Count == 0)
        {
            _builder.AddSelect(isDistinct ? "distinct *" : "*");
        }
    }

    private void ExecuteWhere()
    {
        if (_expression != null && _expression.Count > 0)
        {
            var expression = _expression;
        
            if (_expression.ContainsKey("where"))
            {
                expression = (JsonObject)_expression["where"];
            }

            if (expression.ContainsKey(SPEC))
            {
                ExecuteSpec(expression);
            }
            else if (expression.ContainsKey(RELATION) || expression.ContainsKey(LOGIC))
            {
                ExecuteKeyWhere(expression);
            }
            else
            {
                ExecuteNoKeyWhere(expression);
            }
        }
    }


    private void ExecuteSpec(JsonObject expression)
    {
        var specName = expression[SPEC]?.GetValue<string>();
        if (specName is null) return;

        expression.Remove(SPEC);

        var newExpression = new JsonObject
        {
            { SPEC, specName }
        };

        foreach (var property in expression)
        {
            newExpression.Add(property.Key, property.Value);
        }

        _expression = newExpression;

        var interpreter = LanguageFactory.Build(_expression, _builder);
        _builder.AndWhere(interpreter.Execute());
    }

    private void ExecuteKeyWhere(JsonObject expression)
    {
        var logic = GetLogicOperator(expression);
        var rel = GetRelationOperator(expression);

        _expression.Remove(RELATION);
        _expression.Remove(LOGIC);

        if (logic != null)
        {
            var newExpression = new JsonObject();
            var logicArray = new JsonArray();
            newExpression.Add(logic, logicArray);

            rel ??= "eq";

            foreach (var property in _expression)
            {
                var relObject = new JsonObject();
                var keyValueObject = new JsonObject
                {
                    { property.Key, property.Value }
                };

                relObject.Add(rel, keyValueObject);
                logicArray.Add(relObject);
            }

            var interpreters = logicArray.Where(x => x is not null).Select(item => LanguageFactory.Build(item!, _builder)).ToArray();
            _builder.AndWhere(Method(logic, interpreters));
        }
        else
        {
            var newExpression = new JsonObject
            {
                { rel!, _expression }
            };

            var interpreter = LanguageFactory.Build(newExpression, _builder);
            _builder.AndWhere(interpreter.Execute());
        }
    }

    private void ExecuteNoKeyWhere(JsonObject expression)
    {
        var firstProperty = expression.First();
        var @operator = firstProperty.Key;
        var data = firstProperty.Value;

        if (IsLogicOperator(@operator))
        {
            if (data is not null)
            {
                var interpreters = data.AsArray().Where(x => x is not null).Select(item => LanguageFactory.Build(item!, _builder)).ToArray();
                _builder.AndWhere(Method(@operator, interpreters));
            }
        }
        else if (IsRelationOperator(@operator) || IsSpecification(@operator))
        {
            var newExpression = new JsonObject
            {
                { @operator, data?.DeepClone() }
            };
            var interpreter = LanguageFactory.Build(newExpression, _builder);
            _builder.AndWhere(interpreter.Execute());

            expression.Remove(@operator);
            _expression.Remove(@operator);

            if (expression.Count > 0)
            {
                ExecuteNoKeyWhere(expression);
            }
        }
        else if (_expression.Count == 1)
        {
            var newExpression = new JsonObject
            {
                { "eq", _expression }
            };
            var interpreter = LanguageFactory.Build(newExpression, _builder);
            _builder.AndWhere(interpreter.Execute());
        }
        else
        {
            string Logic = "and";
            string REL = "eq";
            var newExpression = new JsonObject();
            var logicArray = new JsonArray();
            newExpression.Add(Logic, logicArray);

            foreach (var property in _expression)
            {
                if (IsRelationOperator(property.Key))
                {
                    var relExpression = new JsonObject
                    {
                        { property.Key, property.Value }
                    };
                    var interpreter = LanguageFactory.Build(relExpression, _builder);
                    _builder.AndWhere(interpreter.Execute());
                }
                else
                {
                    var relObject = new JsonObject();
                    var keyValueObject = new JsonObject
                    {
                        { property.Key, property.Value }
                    };
                    relObject.Add(REL, keyValueObject);
                    logicArray.Add(relObject);
                }
            }

            var interpreters = logicArray.Where(x => x is not null).Select(item => LanguageFactory.Build(item!, _builder)).ToArray();
            _builder.AndWhere(Method(Logic, interpreters));
        }
    }
}
