using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class EqualsAnyInterpreter : AbstractLanguage
{
    public EqualsAnyInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}