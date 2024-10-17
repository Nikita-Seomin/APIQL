using System.Text;

using SqlKata.Execution;

namespace ApiQL.Language.Specs;

// See https://aka.ms/new-console-template for more information
public class Address : AbstractSpec
{
    private readonly string? _parameter;
    private readonly string? _value;

    /// <summary>
    /// tring parameter - query parameter
    /// </summary>
    /// <returns></returns>
    public Address(string? @parameter, string? value)
    {
        _parameter = @parameter;
        _value = value;
    }

    /// <summary>
    /// QueryBuilder queryBuilder - query builder
    /// </summary>
    /// <param name="queryBuilder"></param>
    /// <returns></returns>
    public string? Match(QueryFactory queryBuilder)
    {
        return string.Empty;
    }
}

// public static class JsonField
// {
//     private static readonly Dictionary<string, string> _alphabet = new(40)
//     {
//         { "А", "a" }, { "Б", "b" }, { "В", "v" }, { "Г", "g" }, { "Д", "d" },
//         { "Е", "e" }, { "Ё", "yo" }, { "Ж", "zh" }, { "З", "z" }, { "И", "i" },
//         { "Й", "j" }, { "К", "k" }, { "Л", "l" }, { "М", "m" }, { "Н", "n" },
//         { "О", "o" }, { "П", "p" }, { "Р", "r" }, { "С", "s" }, { "Т", "t" },
//         { "У", "u" }, { "Ф", "f" }, { "Х", "kh" }, { "Ц", "ts" }, { "Ч", "ch" },
//         { "Ш", "sh" }, { "Щ", "sch" }, { "Ъ", "" }, { "Ы", "y" }, { "Ь", "" },
//         { "Э", "e" }, { "Ю", "yu" }, { "Я", "ya" }, { "а", "a" }, { "б", "b" },
//         { "в", "v" }, { "г", "g" }, { "д", "d" }, { "е", "e" }, { "ё", "yo" },
//         { "ж", "zh" }, { "з", "z" }, { "и", "i" }, { "й", "j" }, { "к", "k" },
//         { "л", "l" }, { "м", "m" }, { "н", "n" }, { "о", "o" }, { "п", "p" },
//         { "р", "r" }, { "с", "s" }, { "т", "t" }, { "у", "u" }, { "ф", "f" },
//         { "х", "kh" }, { "ц", "ts" }, { "ч", "ch" }, { "ш", "sh" }, { "щ", "sch" },
//         { "ъ", "" }, { "ы", "y" }, { "ь", "" }, { "э", "e" }, { "ю", "yu" },
//         { "я", "ya" }, { " ", "_" }, { ".", "" }, { ",", "" }, { "/", "_" },
//         { ":", "" }, { ";", "" }, { "—", "" }, { "–", "" }, { ">", "" }, { "-", "" }, { "'", "" }, { "\"", "" }
//     };
//
//     private static ReadOnlySpan<(string Russian, string Latin)> Alphabet => new (string Russian, string Latin)[]
//     {
//         ("А", "a"), ("Б", "b"), ("В", "v"), ("Г", "g"), ("Д", "d"),
//         ("Е", "e"), ("Ё", "yo"), ("Ж", "zh"), ("З", "z"), ("И", "i"),
//         ("Й", "j"), ("К", "k"), ("Л", "l"), ("М", "m"), ("Н", "n"),
//         ("О", "o"), ("П", "p"), ("Р", "r"), ("С", "s"), ("Т", "t"),
//         ("У", "u"), ("Ф", "f"), ("Х", "kh"), ("Ц", "ts"), ("Ч", "ch"),
//         ("Ш", "sh"), ("Щ", "sch"), ("Ъ", ""), ("Ы", "y"), ("Ь", ""),
//         ("Э", "e"), ("Ю", "yu"), ("Я", "ya"), ("а", "a"), ("б", "b"),
//         ("в", "v"), ("г", "g"), ("д", "d"), ("е", "e"), ("ё", "yo"),
//         ("ж", "zh"), ("з", "z"), ("и", "i"), ("й", "j"), ("к", "k"),
//         ("л", "l"), ("м", "m"), ("н", "n"), ("о", "o"), ("п", "p"),
//         ("р", "r"), ("с", "s"), ("т", "t"), ("у", "u"), ("ф", "f"),
//         ("х", "kh"), ("ц", "ts"), ("ч", "ch"), ("ш", "sh"), ("щ", "sch"),
//         ("ъ", ""), ("ы", "y"), ("ь", ""), ("э", "e"), ("ю", "yu"),
//         ("я", "ya"), (" ", "_"), (".", ""), (",", ""), ("/", "_"),
//         (":", ""), (";", ""), ("—", ""), ("–", ""), (">", ""), ("-", ""), ("'", ""), ("\"", "")
//     };
//
//     /// <summary>
//     /// Поменять потом на сторонний
//     /// </summary>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static string Transliterate(this string value)
//     {
//         ArgumentNullException.ThrowIfNull(value);
//         return string.Concat(value.Select(c => _alphabet.GetValueOrDefault(c.ToString(), c.ToString())));
//     }
//
//     /// <summary>
//     /// Поменять потом на сторонний
//     /// Вариант с использованием Span<char>
//     /// </summary>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static string TransliterateV2(this string value)
//     {
//         ArgumentNullException.ThrowIfNull(value);
//
//         var result = new StringBuilder(value.Length);
//         var span = value.AsSpan();
//
//         for (var i = 0; i < span.Length; i++)
//         {
//             var found = false;
//             for (var j = 0; j < Alphabet.Length; j++)
//             {
//                 if (Alphabet[j].Russian.AsSpan().Equals(span.Slice(i, 1), StringComparison.Ordinal))
//                 {
//                     result.Append(Alphabet[j].Latin);
//                     found = true;
//                     break;
//                 }
//             }
//
//             if (!found)
//             {
//                 result.Append(span[i]);
//             }
//         }
//
//         return result.ToString();
//     }
//
//     /// <summary>
//     /// Check value is float
//     /// </summary>
//     /// <returns></returns>
//     public static bool ContainsString(string value)
//     {
//         ArgumentNullException.ThrowIfNullOrEmpty(value);
//
//
//
//         return true;
//     }
// }