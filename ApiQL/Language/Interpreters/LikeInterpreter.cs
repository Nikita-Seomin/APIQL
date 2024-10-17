using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class LikeInterpreter : AbstractLanguage
{
    public LikeInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}