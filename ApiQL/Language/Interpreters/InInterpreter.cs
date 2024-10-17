using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class InInterpreter : AbstractLanguage
{
    public InInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}