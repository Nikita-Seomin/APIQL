using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class IsNotNullInterpreter : AbstractLanguage
{
    public IsNotNullInterpreter(JsonElement expression, ApiQueryBuilder builder) : base(expression)
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
            var value = JsonField.GetJsonFieldValue(property.Name, property.Value);
            return _builder.isNotNull(value.ToString());

        }
        else if (_expression.ValueKind == JsonValueKind.String)
        {
            return _builder.isNotNull(_expression.GetString());

        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}