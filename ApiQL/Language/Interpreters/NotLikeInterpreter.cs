﻿using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class NotLikeInterpreter : AbstractLanguage
{
    private readonly string _logicOperator;
    public NotLikeInterpreter(JsonElement expression, ApiQueryBuilder builder, string @logicOperator = "and") : base(expression)
    {
        _builder = builder;
        _logicOperator = @logicOperator;
    }

    public override object? Execute()
    {
        if (_expression.ValueKind == JsonValueKind.Object && _expression.EnumerateObject().Any())
        {
            var enumerator = _expression.EnumerateObject().GetEnumerator();
            enumerator.MoveNext();
            var property = enumerator.Current;
            return _builder.NotLike(property.Name, property.Value, _logicOperator);
        }
        
        throw new InvalidOperationException("Expression is not a valid JSON object or is empty.");
    }
}