using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class GarAddressInterpreter : AbstractLanguage
{
    public GarAddressInterpreter(JsonElement expression) : base(expression)
    {
    }

    public override object? Execute()
    {
        throw new NotImplementedException();
    }
}