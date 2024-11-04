using System.Collections.Frozen;
using System.Text.Json;

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
    
    public static object? GetJsonFieldValue(string field, object? value = null, bool isArray = false)
    {
        if (value is JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                string stringValue = jsonElement.GetString();

                if (isArray)
                {
                    var array = stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    // Handle the array if needed
                }

                return stringValue;
                // Add any additional processing for string value if needed
            }
            else if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                long numberValue = jsonElement.GetInt32();

                // if (isArray)
                // {
                //     var array = stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //     // Handle the array if needed
                // }

                return numberValue;
                // Add any additional processing for string value if needed
            }
            else if (jsonElement.ValueKind == JsonValueKind.Object && jsonElement.EnumerateObject().Any())
            {
                var jsonObject = jsonElement;
                JsonProperty firstProperty = jsonObject.EnumerateObject().First();
                string fieldProp = firstProperty.Name;
                field = $"\"getPropertyById\"({field}, '{fieldProp}')";
            
                string fieldType = GetJsonFieldType(firstProperty.Value);
                if (!string.IsNullOrEmpty(fieldType))
                {
                    field += $"::{fieldType}";
                }
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

    public static string ChangeOperatorName(string oper)
    {
        oper = oper.Replace("_","");
        oper = oper.Replace("-","");
        return oper.ToLower() switch
        {
            // "and" => new AndInterpreter(data_),
            // "or" => new OrInterpreter(data_),
            "equals" => "eq",
            "notequals" => "neq",
            // "lt" => new LessThanInterpreter(data_),
            // "lte" => new LessThanOrEqualInterpreter(data_),
            // "gt" => new GreaterThanInterpreter(data_),
            // "gte" => new GreaterThanOrEqualInterpreter(data_),
            // "is_null" => new IsNullInterpreter(data_),
            // "is_not_null" => new IsNotNullInterpreter(data_),
            // "like" => new LikeInterpreter(data_),
            // "not_like" => new NotLikeInterpreter(data_),
            // "in" => new InInterpreter(data_),
            // "not_in" => new NotInInterpreter(data_),
            // "equals_any" => new EqualsAnyInterpreter(data_),
            // "not_equals_any" => new NotEqualsAnyInterpreter(data_),
            // "equals_all" => new EqualsAllInterpreter(data_),
            // "not_equals_all" => new NotEqualsAllInterpreter(data_),
            // "spec" => new SpecInterpreter(data),
            // "address" => new AddressInterpreter(data_),
            // "gar_address" => new GarAddressInterpreter(data_),
            // "search" or "se" => new SearchInterpreter(data_),
            _ => throw new ArgumentException($"Operator \"{oper}\" not found")
        };
    }
}
