using System.Text.Json;
using System.Text.Json.Nodes;
using ApiQL.Language.Interpreters;

namespace ApiQL.Language;

/// <summary>
/// Class LanguageFactory
/// </summary>
internal class LanguageFactory
{
    public static ILanguage Build(JsonNode data, ApiQueryBuilder builder, string specFlag = null, string @logicOperator = "and")
    {
        
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "Provided JSON data is null.");
        }

        try
        {
            string? @operator = null;
            // Преобразуйте JsonNode в JsonElement
            JsonElement element = data.Deserialize<JsonElement>();

            JsonElement data_;
            if (@operator is not null)
            {
                data_ = element;
            }
            else
            {
                if (element.ValueKind == JsonValueKind.Object && element.EnumerateObject().Any())
                {
                    @operator = element.EnumerateObject().First().Name;
                    data_ = element.GetProperty(@operator);
                }
                else
                {
                    throw new ArgumentException("Provided data is not a valid JSON object or it is empty.");
                }
            }


            return @operator switch
            {
                // "and" => new AndInterpreter(data_),
                // "or" => new OrInterpreter(data_),
                "eq" => new EqualsInterpreter(data_, builder),
                "neq" => new NotEqualsInterpreter(data_, builder, specFlag),
                "lt" => new LessThanInterpreter(data_, builder),
                "lte" => new LessThanOrEqualInterpreter(data_, builder),
                "gt" => new GreaterThanInterpreter(data_, builder),
                "gte" => new GreaterThanOrEqualInterpreter(data_, builder),
                "is_null" => new IsNullInterpreter(data_, builder),
                "is_not_null" => new IsNotNullInterpreter(data_, builder),
                "like" => new LikeInterpreter(data_, builder, @logicOperator, specFlag),
                "not_like" => new NotLikeInterpreter(data_, builder, @logicOperator, specFlag),
                "in" => new InInterpreter(data_, builder, @logicOperator),
                "not_in" => new NotInInterpreter(data_, builder, @logicOperator, specFlag),
                "between" => new BetweenInterpreter(data_, builder, @logicOperator, specFlag),
                // "equals_any" => new EqualsAnyInterpreter(data_),
                // "not_equals_any" => new NotEqualsAnyInterpreter(data_),
                // "equals_all" => new EqualsAllInterpreter(data_),
                // "not_equals_all" => new NotEqualsAllInterpreter(data_),
                // "spec" => new SpecInterpreter(data_, builder),
                // "address" => new AddressInterpreter(data_),
                // "gar_address" => new GarAddressInterpreter(data_),
                // "search" or "se" => new SearchInterpreter(data_),
                _ => throw new ArgumentException($"Interpreter \"{@operator}\" not found")
            };

        }
        catch (Exception ex)
        {
            throw new ArgumentException("Unable to deserialize JsonNode to JsonElement.", ex);
        }
    }
}