#pragma warning disable CA1854

namespace ApiQL.Language;

internal class ParametersStorage
{
    private static Dictionary<string, int> parameters = new Dictionary<string, int>();

    public static int? Get(string name)
    {
        return parameters.TryGetValue(name, out var value) ? value : null;
    }

    public static void Add(string name)
    {
        if (parameters.ContainsKey(name))
        {
            parameters[name]++;
        }
        else
        {
            parameters[name] = 1;
        }
    }

    public static void Reset()
    {
        parameters.Clear();
    }
}