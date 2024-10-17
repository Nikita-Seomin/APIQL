using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class EqualsAllInterpreter : AbstractLanguage
{
    public EqualsAllInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}