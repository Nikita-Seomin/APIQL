namespace ApiQL.Language.Specs;

using System;
using SqlKata;
using SqlKata.Execution;
using global::ApiQL.Language;

public class Equals : AbstractSpec
{
    
    private string parameter;
    private object value;

    public Equals(string parameter, object value)
    {
        this.parameter = parameter;
        this.value = value;
    }

    public string Match(QueryFactory queryFactory)
    {
        string? fieldName = JsonField.GetJsonFieldName(parameter, value);
        string? fieldValue = JsonField.GetJsonFieldValue(parameter, value);
        object jsonValue = JsonField.GetJsonPropertyValue(value);

        // SqlKata позволяет использовать параметризацию в запросе
        var query = new Query("your_table_name")
            .Where(fieldValue, jsonValue);
        
        // Если вам нужно выполнить данный запрос в базе данных, используйте:
        // var result = await queryFactory.Query(query).GetAsync();

        return $"{fieldValue} = @{fieldName}"; // Используйте формат строки для возвращения условия
    }
}
