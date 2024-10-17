using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class IsNullInterpreter : AbstractLanguage
{
    public IsNullInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}