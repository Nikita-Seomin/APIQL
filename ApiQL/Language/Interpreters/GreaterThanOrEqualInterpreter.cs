using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class GreaterThanOrEqualInterpreter : AbstractLanguage
{
    public GreaterThanOrEqualInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}