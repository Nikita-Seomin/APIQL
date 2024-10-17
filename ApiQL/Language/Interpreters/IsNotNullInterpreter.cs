using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class IsNotNullInterpreter : AbstractLanguage
{
    public IsNotNullInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}