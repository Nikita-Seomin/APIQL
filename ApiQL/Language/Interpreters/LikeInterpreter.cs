using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LikeInterpreter : AbstractLanguage
{
    public LikeInterpreter(JsonElement expression, ApiQueryBuilder builder) : base(expression)
    {
        _builder = builder;
    }

    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            return _builder.like(property.Name, property.Value);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}