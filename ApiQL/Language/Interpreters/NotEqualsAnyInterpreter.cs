using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotEqualsAnyInterpreter : AbstractLanguage
{
    public NotEqualsAnyInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}