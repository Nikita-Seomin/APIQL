using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class InInterpreter : AbstractLanguage
{
    private readonly string _logicOperator;
    public InInterpreter(JsonElement expression, ApiQueryBuilder builder, string @logicOperator = "and") : base(expression)
    {
        _builder = builder;
        _logicOperator = @logicOperator;
    }

    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            return _builder.In(property.Name, property.Value, _logicOperator);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}