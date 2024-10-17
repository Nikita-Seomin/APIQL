using System.Reflection;
using System.Text.Json.Nodes;

#pragma warning disable S3011

namespace ApiQL.Language;

internal static class SpecialFunctionsBuilder
{
    internal static string? Execute(string key, JsonObject value)
    {
        if (!value.ContainsKey("func")) return null;

        var function = value["func"]?.GetValue<string>();
        if (function is null) return null;

        var method = typeof(SpecialFunctionsBuilder).GetMethod(function, BindingFlags.NonPublic | BindingFlags.Static);

        return method is null ? null : (string?)method.Invoke(null, [key, value]);
    }

    internal static string First(string key, JsonObject value)
    {
        var field = key == "*" ? "(*)" : $"(\"{key}\")";

        field = $"(array_agg{field} order by sort)[1]";

        if (value.ContainsKey("as"))
        {
            field += $" as \"{value["as"]!.GetValue<string>()}\"";
        }

        return field;
    }

    internal static string Last(string key, JsonObject value)
    {
        var field = key == "*" ? "(*)" : $"(\"{key}\")";

        field = $"(array_agg{field} order by sort)[array_upper(array_agg{field} order by sort, 1)]";

        if (value.ContainsKey("as"))
        {
            field += $" as \"{value["as"]!.GetValue<string>()}\"";
        }

        return field;
    }
}