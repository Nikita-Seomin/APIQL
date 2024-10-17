using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class SearchInterpreter : AbstractLanguage
{
    public SearchInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}