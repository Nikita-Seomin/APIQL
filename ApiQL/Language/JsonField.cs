using System.Collections.Frozen;

namespace ApiQL.Language;

public static class JsonField
{
    /// <summary>
    /// Transliterate to latin
    /// </summary>
    /// <param name="value">The string to transliterate</param>
    /// <returns>The transliterated string</returns>
    public static string Transliterate(this string value)
    {
        return string.Concat(value.Select(c => _alphabet.TryGetValue(c, out var replacement) ? replacement : c.ToString()));
    }

    private static readonly FrozenDictionary<char, string> _alphabet = new Dictionary<char, string>
    {
        {'А', "a"}, {'Б', "b"}, {'В', "v"}, {'Г', "g"}, {'Д', "d"},
        {'Е', "e"}, {'Ё', "yo"}, {'Ж', "zh"}, {'З', "z"}, {'И', "i"},
        {'Й', "j"}, {'К', "k"}, {'Л', "l"}, {'М', "m"}, {'Н', "n"},
        {'О', "o"}, {'П', "p"}, {'Р', "r"}, {'С', "s"}, {'Т', "t"},
        {'У', "u"}, {'Ф', "f"}, {'Х', "kh"}, {'Ц', "ts"}, {'Ч', "ch"},
        {'Ш', "sh"}, {'Щ', "sch"}, {'Ъ', ""}, {'Ы', "y"}, {'Ь', ""},
        {'Э', "e"}, {'Ю', "yu"}, {'Я', "ya"},
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
        {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
        {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
        {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
        {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
        {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
        {'э', "e"}, {'ю', "yu"}, {'я', "ya"},
        {' ', "_"}, {'.', ""}, {',', ""}, {'/', "_"}, {':', ""},
        {';', ""}, {'—', ""}, {'–', ""}, {'>', ""}, {'-', ""}, {'\'', ""}, {'"', ""}
    }.ToFrozenDictionary();

    public static string TransliterateV2(this string? strValue)
    {
        var value = strValue.AsSpan();
        var result = new char[value.Length * 2]; // Worst case scenario
        var resultSpan = result.AsSpan();
        var writeIndex = 0;

        foreach (var c in value)
        {
            if (_alphabet.TryGetValue(c, out var replacement))
            {
                replacement.AsSpan().CopyTo(resultSpan[writeIndex..]);
                writeIndex += replacement.Length;
            }
            else
            {
                resultSpan[writeIndex++] = c;
            }
        }

        return new string(resultSpan[..writeIndex]);
    }
    
    
    public static string? GetJsonFieldName(string field, object value)
    {
        if (value is Dictionary<string, object> dictionary && dictionary.Count > 0)
        {
            // Получаем первый ключ словаря
            string firstKey = new List<string>(dictionary.Keys)[0];
            field = $"{field}_{firstKey}_";
        }

        // Предполагаем, что ParametersStorage - это класс, с методами Get и Add
        var parameter = ParametersStorage.Get(field);
        ParametersStorage.Add(field);

        if (parameter != null)
        {
            field += parameter;
        }

        return Transliterate(field);
    }
    
    public static string? GetJsonFieldValue(string field, object? value = null, bool isArray = false)
    {
        if (value is Dictionary<string, object> dictionary && dictionary.Count > 0 && dictionary.Keys.First() != null)
        {
            // Получаем первый ключ словаря
            string fieldProp = new List<string>(dictionary.Keys)[0];
            field = $"\"getPropertyById\"({field}, '{fieldProp}')";
                
            if (isArray && dictionary[fieldProp] is string propertyValue)
            {
                // Преобразуем строку в массив
                dictionary[fieldProp] = propertyValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            string fieldType = GetJsonFieldType(dictionary[fieldProp]);
            if (!string.IsNullOrEmpty(fieldType))
            {
                field += $"::{fieldType}";
            }
        }

        if (value is bool)
        {
            field += "::bool";
        }

        return field;
    }
    
    public static object GetJsonPropertyValue(object value)
    {
        if (value is IDictionary<string, object> dictionary && !dictionary.ContainsKey("0"))
        {
            value = dictionary.First().Value;
        }
        return value;
    }

    
    
    private static string GetJsonFieldType(object value)
    {
        if (IsBoolean(value))
        {
            return "boolean";
        }
        if (IsInt(value))
        {
            return "int";
        }
        if (IsFloat(value))
        {
            return "float";
        }
        return string.Empty;
    }
    
    
    private static bool IsBoolean(object value)
    {
        if (value is bool)
        {
            return true;
        }

        if (value is string strValue)
        {
            return strValue == "true" || strValue == "false";
        }

        return false;
    }

    private static bool IsInt(object? value)
    {
        if (value is Array array)
        {
            return IsInt(array.GetValue(0)); // Проверяем первый элемент массива
        }

        return value is int || (value is string str && int.TryParse(str, out _));
    }

    private static bool IsFloat(object? value)
    {
        if (value is Array array)
        {
            return IsFloat(array.GetValue(0)); // Проверяем первый элемент массива
        }

        if (value is float || value is double)
        {
            return true; // Если это float или double
        }

        if (value is string str)
        {
            return float.TryParse(str, out _) || double.TryParse(str, out _); // Проверяем строку
        }

        return false; // В других случаях
    }



}
