﻿using System.Text.Json;
using ApiQL.Language;

namespace ApiQL.Language.Interpreters;

internal class NotEqualsInterpreter : AbstractLanguage
{
    
    public NotEqualsInterpreter(JsonElement expression, ApiQueryBuilder builder) : base(expression)
    {
        _builder = builder;
    }
    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            string jsonString = $"{{\"{property.Name}\":\"{property.Value}\"}}";
            var dictionary = new Dictionary<string, object>();
            dictionary.Add(property.Name, property.Value);
            // Здесь создаем объект с анонимными типами для использования в `Where`
            var parameters = new Dictionary<string, object>()
            {
                { property.Name, property.Value },
            };

            return _builder.neq(property.Name, property);

        }

        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}