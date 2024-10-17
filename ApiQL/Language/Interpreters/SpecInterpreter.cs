using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class SpecInterpreter : AbstractLanguage
{
    public SpecInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}