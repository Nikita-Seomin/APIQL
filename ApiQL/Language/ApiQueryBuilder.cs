using System.Collections;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;

namespace ApiQL.Language;
using System;
using ApiQL.Language;

/// <summary>
/// Interface InterpreterInterface
/// </summary>
public class ApiQueryBuilder
{
    /// <summary>
    /// Query builder implementation
    /// </summary>
    private readonly Query _builder;

    /// <summary>
    /// Expression implementation
    /// </summary>
    // private readonly Query _expression;

    private readonly Compiler compiler = new PostgresCompiler();

    /// <summary>
    /// Construct query builder
    /// </summary>
    /// <param name="builder">decorated query builder</param>
    public ApiQueryBuilder(Query builder)
    {
        ParametersStorage.Reset();
        _builder = builder;
        // _expression = _builder.Query();
    }

    internal object AndX(object expressions)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Execute logic AND expression with expression builder
    /// </summary>
    /// <param name="expressions">Nested expressions</param>
    /// <returns>CompositeExpression if expressions provided, null otherwise</returns>
    public Query? AndX(params Query[]? expressions)
    {
        if (expressions == null || expressions.Length == 0)
        {
            return null; // Возвращаем null, если нет выражений
        }

        // Создаем новое выражение Query для объединения
        var combinedQuery = new Query();

        // Объединяем выражения через AND
        foreach (var expression in expressions)
        {
            combinedQuery = combinedQuery.Where(expression);
        }

        return combinedQuery;
    }



    /// <summary>
    /// Add field to SELECT part
    /// </summary>
    /// <param name="field">field name</param>
    public void AddSelect(string field)
    {
        _builder.Select(field);
    }
    
    /// <summary>
    /// Set grouping
    /// </summary>
    /// <param name="fields">group fields</param>
    public void AddGroupBy(string[] fields)
    {
        _builder.GroupBy(fields);
    }
    
    /// <summary>
    /// Set offset part
    /// </summary>
    /// <param name="offset">offset</param>
    public void SetFirstResult(int offset)
    {
        _builder.Offset(offset);
    }
    
    /// <summary>
    /// Set limit part
    /// </summary>
    /// <param name="limit">limit</param>
    public void SetMaxResults(int limit)
    {
        _builder.Limit(limit); 
    }
    
    
    /// <summary>
    /// Set ordering
    /// </summary>
    /// <param name="field">field</param>
    /// <param name="orderType">order type asc|desc</param>
    public void AddOrderBy(string field, string orderType = null)
    {
        if (string.IsNullOrEmpty(orderType) || orderType.Equals("asc", StringComparison.OrdinalIgnoreCase))
        {
            _builder.OrderBy(field); // По умолчанию сортировка по возрастанию
        }
        else if (orderType.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            _builder.OrderByDesc(field); // Сортировка по убыванию
        }
    }
    
    
    /// <summary>
    /// Return query part
    /// </summary>
    /// <param name="part">Query part</param>
    /// <returns>List of query parts or an empty list on exception</returns>
    public List<string> GetQueryPart(string part)
    {
        try
        {
            // Получаем SQL-запрос в виде строки
            var sql = _builder.ForPostgreSql(expression => _builder); // Используем метод ToSql для получения SQL

            // Поскольку в вашем оригинальном коде есть возврат "not empty",
            // мы просто возвращаем список с текстом, если запрос не пустой
            return new List<string> { "not empty" };
        }
        catch (Exception)
        {
            return new List<string>(); // Возвращаем пустой список в случае ошибки
        }
    }
    
    
    /**
     * Set where
     *
     * @param object where - where
     */
    public void AndWhere(object where)
    {
        if (where != null)
        {
            // Предположим, что `where` представляет собой условия, например, в виде анонимного объекта или других форматов
            _builder.Where(where); // Или можете реализовать логику для обработки условий
        }
    }
    
    public Query Neq(string field, object value, string specFlag = null)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(q =>
        {
            q.WhereNot(fieldName, fieldValue);
           if (specFlag != "spec") q.OrWhereNull(fieldName);
            return q;
        });
        return _builder;
    }
    
    public Query Eq(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        object parameterValue = JsonField.GetJsonPropertyValue(value);
        var dictionary = new Dictionary<string, object>();
        dictionary.Add(fieldName, fieldValue);
        _builder.Where(dictionary);
        return _builder;
       
    }
    
    
    public Query gt(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, ">", fieldValue);
        return _builder;
    }
    
    public Query gte(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, ">=", fieldValue);
        return _builder;
    }
    
    public Query lt(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, "<", fieldValue);
        return _builder;
    }
    
    public Query lte(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, "<=", fieldValue);
        return _builder;
    }
    
    
    public Query isNull(string field)
    {
        // string fieldName = JsonField.GetJsonFieldName(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.WhereNull(field);
        return _builder;
    }
    
    
    public Query isNotNull(string field)
    {
        // string fieldName = JsonField.GetJsonFieldName(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.WhereNotNull(field);
        return _builder;
    }
    
    
    public Query Like(string field, object value, string logicOperator)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator))
            _builder.OrWhere(q 
                => q.WhereLike(fieldName, fieldValue, false, "*"));
            // _builder.OrWhereLike(fieldName, fieldValue, false, "*");
        else
            _builder.WhereLike(fieldName, fieldValue, false, "*");
        return _builder;
    }
    
    
    public Query NotLike(string field, object value, string logicOperator)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator))
            _builder.OrWhere(q 
                => q.WhereNotLike(fieldName, fieldValue, false, "*"));
        // _builder.OrWhereLike(fieldName, fieldValue, false, "*");
        else
            _builder.WhereNotLike(fieldName, fieldValue, false, "*");
        return _builder;
    }

}
