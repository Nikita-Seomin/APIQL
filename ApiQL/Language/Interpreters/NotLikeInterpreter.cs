using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotLikeInterpreter : AbstractLanguage
{
    public NotLikeInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}