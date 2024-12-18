﻿using System.Collections;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata.Extensions;
using System.Text.Json;
namespace ApiQL.Language;
using System;
using ApiQL.Language;

/// <summary>
/// Interface InterpreterInterface
/// </summary>
public class ApiQueryBuilder
{
    /// <summary>
    /// Query builder implementation
    /// </summary>
    private readonly Query _builder;

    /// <summary>
    /// Expression implementation
    /// </summary>
    // private readonly Query _expression;

    private readonly Compiler compiler = new PostgresCompiler();

    /// <summary>
    /// Construct query builder
    /// </summary>
    /// <param name="builder">decorated query builder</param>
    public ApiQueryBuilder(Query builder)
    {
        ParametersStorage.Reset();
        _builder = builder;
        // _expression = _builder.Query();
    }

    internal object AndX(object expressions)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Execute logic AND expression with expression builder
    /// </summary>
    /// <param name="expressions">Nested expressions</param>
    /// <returns>CompositeExpression if expressions provided, null otherwise</returns>
    public Query? AndX(params Query[]? expressions)
    {
        if (expressions == null || expressions.Length == 0)
        {
            return null; // Возвращаем null, если нет выражений
        }

        // Создаем новое выражение Query для объединения
        var combinedQuery = new Query();

        // Объединяем выражения через AND
        foreach (var expression in expressions)
        {
            combinedQuery = combinedQuery.Where(expression);
        }

        return combinedQuery;
    }



    /// <summary>
    /// Add field to SELECT part
    /// </summary>
    /// <param name="field">field name</param>
    public void AddSelect(string field)
    {
        _builder.Select(field);
    }
    
    /// <summary>
    /// Set grouping
    /// </summary>
    /// <param name="fields">group fields</param>
    public void AddGroupBy(string[] fields)
    {
        _builder.GroupBy(fields);
    }
    
    /// <summary>
    /// Set offset part
    /// </summary>
    /// <param name="offset">offset</param>
    public void SetFirstResult(int offset)
    {
        _builder.Offset(offset);
    }
    
    /// <summary>
    /// Set limit part
    /// </summary>
    /// <param name="limit">limit</param>
    public void SetMaxResults(int limit)
    {
        _builder.Limit(limit); 
    }
    
    
    /// <summary>
    /// Set ordering
    /// </summary>
    /// <param name="field">field</param>
    /// <param name="orderType">order type asc|desc</param>
    public void AddOrderBy(string field, string orderType = null)
    {
        if (string.IsNullOrEmpty(orderType) || orderType.Equals("asc", StringComparison.OrdinalIgnoreCase))
        {
            _builder.OrderBy(field); // По умолчанию сортировка по возрастанию
        }
        else if (orderType.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            _builder.OrderByDesc(field); // Сортировка по убыванию
        }
    }
    
    
    /// <summary>
    /// Return query part
    /// </summary>
    /// <param name="part">Query part</param>
    /// <returns>List of query parts or an empty list on exception</returns>
    public List<string> GetQueryPart(string part)
    {
        try
        {
            // Получаем SQL-запрос в виде строки
            var sql = _builder.ForPostgreSql(expression => _builder); // Используем метод ToSql для получения SQL

            // Поскольку в вашем оригинальном коде есть возврат "not empty",
            // мы просто возвращаем список с текстом, если запрос не пустой
            return new List<string> { "not empty" };
        }
        catch (Exception)
        {
            return new List<string>(); // Возвращаем пустой список в случае ошибки
        }
    }
    
    
    /**
     * Set where
     *
     * @param object where - where
     */
    public void AndWhere(object where)
    {
        if (where != null)
        {
            // Предположим, что `where` представляет собой условия, например, в виде анонимного объекта или других форматов
            _builder.Where(where); // Или можете реализовать логику для обработки условий
        }
    }
    
    public Query Neq(string field, object value, string specFlag = null)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(q =>
        {
            q.WhereNot(fieldName, fieldValue);
           if (specFlag != "spec") q.OrWhereNull(fieldName);
            return q;
        });
        return _builder;
    }
    
    public Query Eq(string field, object value, string logicOperator )
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // object parameterValue = JsonField.GetJsonPropertyValue(value);
        // var dictionary = new Dictionary<string, object>();
        // dictionary.Add(fieldName, fieldValue);
        if (AbstractLanguage.IsOrOperator(logicOperator))
            _builder.OrWhere(fieldName, fieldValue);
        else
            _builder.Where( fieldName, fieldValue);
        return _builder;
       
    }
    
    
    public Query gt(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, ">", fieldValue);
        return _builder;
    }
    
    public Query gte(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, ">=", fieldValue);
        return _builder;
    }
    
    public Query lt(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, "<", fieldValue);
        return _builder;
    }
    
    public Query lte(string field, object value)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.Where(fieldName, "<=", fieldValue);
        return _builder;
    }
    
    
    public Query isNull(string field)
    {
        // string fieldName = JsonField.GetJsonFieldName(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.WhereNull(field);
        return _builder;
    }
    
    
    public Query isNotNull(string field)
    {
        // string fieldName = JsonField.GetJsonFieldName(field, value);
        
        // Строим условия с помощью SqlKata
        _builder.WhereNotNull(field);
        return _builder;
    }
    
    
    public Query ILike(string field, object value, string logicOperator, string? specFlag)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator))
            _builder.OrWhere(q                                                                          
                => specFlag is null ?                                                                        // если запрос НЕ со spec
                    q.WhereLike(fieldName, fieldValue, false, "*") :         // добавляем ESCAPE "*"
                    q.WhereLike(fieldName, fieldValue, false));                                  // Если есть specFlag, то БЕЗ ESCAPE "*"
        else
            _builder.Where(q                                                                            // если в составе оператора and
                => specFlag is null ?                                                                        // если запрос НЕ со spec
                    q.WhereLike(fieldName, fieldValue, false, "*") :      // добавляем ESCAPE "*"
                    q.WhereLike(fieldName, fieldValue, false));                                // Если есть specFlag, то БЕЗ ESCAPE "*"
        return _builder;
    }
    
    
    
    public Query Like(string field, object value, string logicOperator, string? specFlag)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator))
            _builder.OrWhere(q                                                                          
                =>  q.WhereLike(fieldName, fieldValue));                      
        else
            _builder.Where(q                                                                          
                =>  q.WhereLike(fieldName, fieldValue));                            
        return _builder;
    }
    
    
    public Query NotLike(string field, object value, string logicOperator, string? specFlag)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator))                                                     // если в составе оператора or
            _builder.OrWhere(q                                                                          
                => specFlag is null ?                                                                        // если запрос НЕ со spec
                    q.WhereNotLike(fieldName, fieldValue, false, "*") :      // добавляем ESCAPE "*"
                    q.WhereNotLike(fieldName, fieldValue, false));                               // Если есть specFlag, то БЕЗ ESCAPE "*"
        else
            _builder.Where(q                                                                            // если в составе оператора and
                => specFlag is null ?                                                                        // если запрос НЕ со spec
                    q.WhereNotLike(fieldName, fieldValue, false, "*") :      // добавляем ESCAPE "*"
                    q.WhereNotLike(fieldName, fieldValue, false));                               // Если есть specFlag, то БЕЗ ESCAPE "*"
        return _builder;
    }
    
    
    public Query In(string field, object value, string logicOperator)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        var dataArr = fieldValue?.ToString()!.Split(',');
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator)) // если в составе оператора or
            _builder.OrWhere(q
                => q.WhereIn(fieldName,dataArr));     
        else
            _builder.Where(q
                => q.WhereIn(fieldName, dataArr));   
        return _builder;
    }
    
    
    public Query NotIn(string field, object value, string logicOperator)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        
        var dataArr = fieldValue?.ToString()!.Split(',');
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator)) // если в составе оператора or
            _builder.OrWhere(q
                => q.WhereNotIn(fieldName,dataArr));     
        else
            _builder.Where(q
                => q.WhereNotIn(fieldName, dataArr));   
        return _builder;
    }
    
    
    public Query Between(string field, JsonElement value, string logicOperator)
    {
        string fieldName = JsonField.GetJsonFieldName(field, value);
        object fieldValue = JsonField.GetJsonFieldValue(field, value);
        // Получаем значения из JsonElement как массив 
        var typedArray = JsonField.ConvertDataToArray(fieldValue, value);
        if (typedArray.Length < 2)
            throw new InvalidOperationException("Too few input parameters");
        // Строим условия с помощью SqlKata
        if (AbstractLanguage.IsOrOperator(logicOperator)) // если в составе оператора or
            _builder.OrWhere(q
                => q.WhereBetween(fieldName,typedArray[0],typedArray[1]));     
        else
            _builder.Where(q
                => q.WhereBetween(fieldName, typedArray[0],typedArray[1]));   
        return _builder;
    }
        
        

}
