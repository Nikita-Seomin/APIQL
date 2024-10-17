using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class GreaterThanInterpreter : AbstractLanguage
{
    public GreaterThanInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}