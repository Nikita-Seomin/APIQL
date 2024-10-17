using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LessThanInterpreter : AbstractLanguage
{
    public LessThanInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}