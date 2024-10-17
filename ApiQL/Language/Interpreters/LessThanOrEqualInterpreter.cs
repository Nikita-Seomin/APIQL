using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LessThanOrEqualInterpreter : AbstractLanguage
{
    public LessThanOrEqualInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}