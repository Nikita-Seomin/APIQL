using SqlKata.Execution;

namespace ApiQL.Language.Specs;

internal class StartsWith : AbstractSpec
{
    private readonly string? _parameter;
    private readonly string? _value;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="parameter">query parameter</param>
    /// <param name="value">parameter value</param>
    /// <param name="resetParams"></param>
    internal StartsWith(string? @parameter, string? value, bool resetParams = false)
    {
        _parameter = parameter;
        _value = value;
    }

    internal string? Match(QueryFactory queryBuilder)
    {
        var fieldname = Language.JsonField.GetJsonFieldName(_parameter, _value);
        var fieldValue = Language.JsonField.GetJsonFieldValue(_parameter, _value);

        return null;
    }
}
