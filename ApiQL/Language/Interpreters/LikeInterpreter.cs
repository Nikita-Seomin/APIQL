using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LikeInterpreter : AbstractLanguage
{
    private string logicOperator;
    public LikeInterpreter(JsonElement expression, ApiQueryBuilder builder, string @logicOperator = "and") : base(expression)
    {
        _builder = builder;
        this.logicOperator = @logicOperator;
    }

    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            return _builder.Like(property.Name, property.Value, logicOperator);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}