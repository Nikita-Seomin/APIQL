using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotInInterpreter : AbstractLanguage
{
    public NotInInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}