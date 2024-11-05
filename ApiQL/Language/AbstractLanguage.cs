using System.Text.Json;
using System.Text.Json.Nodes;

namespace ApiQL.Language;

public abstract class AbstractLanguage : ILanguage
{
    /// <summary>
    /// Relational operators
    /// </summary>
    private static readonly string[] RELATION_OPERATORS = [
                                                            "eq", "neq", "lt", "lte", "gt", "gte", "is_null", "is_not_null",
                                                            "like", "not_like", "in", "not_in", "equals_any", "not_equals_any",
                                                            "equals_all", "not_equals_all", "search", "se"
                                                          ];

    /// <summary>
    /// Relation key in expression
    /// </summary>
    public const string RELATION = "rel";

    /// <summary>
    /// Logic operators
    /// </summary>
    private static readonly string[] LOGIC_OPERATORS = ["and", "or"];

    /// <summary>
    /// Logic key in expression
    /// </summary>
    public const string LOGIC = "logic";

    /// <summary>
    /// Spec key in expression
    /// </summary>
    public const string SPEC = "spec";

    /// <summary>
    /// Expression to be interpreted
    /// </summary>
    protected JsonElement _expression;

    /// <summary>
    /// Query builder decorator
    /// </summary>
    protected ApiQueryBuilder? _builder = null;

    /// <summary>
    /// Specification mappings
    /// </summary>
    protected static List<object> _specs = [];

    protected AbstractLanguage() { }

    /// <summary>
    /// Construct abstract interpreter
    /// </summary>
    /// <param name="expression">expression to be interpreted</param>
    public AbstractLanguage(JsonElement expression)
    {
        _expression = expression;
    }
    
    /// <summary>
    /// Construct abstract interpreter
    /// </summary>
    /// <param name="operator">expression to be interpreted</param>
    public static bool IsAndOperator(string @operator)
    {
        return @operator == LOGIC_OPERATORS[0];
    }
    
    
    /// <summary>
    /// Construct abstract interpreter
    /// </summary>
    /// <param name="operator">expression to be interpreted</param>
    public static bool IsOrOperator(string @operator)
    {
        return @operator == LOGIC_OPERATORS[1];
    }

    /// <summary>
    /// Check if it is unary operator
    /// </summary>
    /// <param name="operator">operator to be checked</param>
    /// <returns></returns>
    public static bool IsRelationOperator(string @operator)
    {
        return Array.Exists(RELATION_OPERATORS, element => element.Equals(@operator, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Check if it is variadic operator
    /// </summary>
    /// <param name="operator">operator to be checked</param>
    /// <returns>bool</returns>
    public static bool IsLogicOperator(string @operator)
    {
        return Array.Exists(LOGIC_OPERATORS, element => element.Equals(@operator, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Get relation operator from expression
    /// </summary>
    /// <param name="expression">expression with operator</param>
    /// <returns>string?</returns>
    public static string? GetRelationOperator(JsonObject expression)
    {
        // Проверяем, содержит ли JsonObject ключ RELATION
        if (expression.ContainsKey(RELATION) && expression[RELATION] is JsonValue jsonValue && jsonValue.TryGetValue(out string? stringValue) && IsRelationOperator(stringValue))
        {
            return stringValue; // Возвращаем значение, если оно является отношенческим оператором
        }

        return null; // Возвращаем null, если ключ не найден или значение не является операцией отношения
    }

    // public static string? GetRelationOperator(Dictionary<string, object> expression)
    // {
    //     if (expression.TryGetValue(RELATION, out var value) && IsRelationOperator((string)value))
    //     {
    //         return (string)value;
    //     }
    //
    //     return null;
    // }

    /// <summary>
    /// Get logic operator from expression
    /// </summary>
    /// <param name="expression">expression with operator</param>
    /// <returns>string?</returns>
    ///
    public static string? GetLogicOperator(JsonObject expression)
    {
        // Проверяем, содержит ли JsonObject ключ LOGIC
        if (expression.ContainsKey(LOGIC) && expression[LOGIC] is JsonValue jsonValue && jsonValue.TryGetValue(out string? stringValue) && IsLogicOperator(stringValue))
        {
            return stringValue; // Возвращаем значение, если оно является логическим оператором
        }

        return null; // Возвращаем null, если ключ не найден или значение не является логическим оператором
    }
    // public static string? GetLogicOperator(Dictionary<string, object> expression)
    // {
    //     if (expression.TryGetValue(LOGIC, out var value) && IsLogicOperator((string)value))
    //     {
    //         return (string)value;
    //     }
    //
    //     return null;
    // }

    /// <summary>
    /// Check if it is specification
    /// </summary>
    /// <param name="operator">operator to be checked</param>
    /// <returns>bool</returns>
    public static bool IsSpecification(string @operator)
    {
        return @operator.Equals("spec", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Apply handler method based on its type
    /// </summary>
    /// <param name="name">method name</param>
    /// <param name="args">interpreters</param>
    /// <returns>mixed</returns>
    /// <exception cref="ArgumentException"></exception>
    public object Method(string name = null, params object[] args)
    {
        // var interpreters = Array.ConvertAll(args, interpreter => interpreter.Execute());

        /*
        switch (name)
        {
            case "or":
                return _builder.OrX(interpreters);
            case "and":
                return _builder.AndX(interpreters);
            case "spec":
                return _builder.Spec(interpreters);
            default:
                throw new ArgumentException($"Method \"{name}\" not found");
        }
        */

        return null;
    }

    public abstract object? Execute();
}