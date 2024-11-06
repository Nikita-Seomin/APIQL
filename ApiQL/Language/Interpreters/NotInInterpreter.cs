using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotInInterpreter : AbstractLanguage
{
    private readonly string _logicOperator;
    private readonly string _specFlag;
    public NotInInterpreter(JsonElement expression, ApiQueryBuilder builder, string @logicOperator = "and", string specFlag = null!) : base(expression)
    {
        _builder = builder;
        _logicOperator = @logicOperator;
        _specFlag = specFlag;
    }

    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            return _builder.NotIn(property.Name, property.Value, _logicOperator);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}