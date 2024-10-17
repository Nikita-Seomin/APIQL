using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class AddressInterpreter : AbstractLanguage
{
    public AddressInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}