using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotEqualsAllInterpreter : AbstractLanguage
{
    public NotEqualsAllInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}