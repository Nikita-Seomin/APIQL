using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LikeInterpreter : AbstractLanguage
{
    private readonly string _logicOperator;
    private readonly string _specFlag;
    public LikeInterpreter(JsonElement expression, ApiQueryBuilder builder, string @logicOperator = "and", string specFlag = null!) : base(expression)
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
            return _builder.Like(property.Name, property.Value, _logicOperator, _specFlag);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}