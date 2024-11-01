// using System.Text.Json;
// using ApiQL.Language;
//
// namespace ApiQL.Language.Interpreters;
//
// internal class SpecInterpreter : AbstractLanguage
// {
//     public SpecInterpreter(JsonElement expression, ApiQueryBuilder builder) : base(expression)
//     {
//         _builder = builder;
//     }
//
//     public override object? Execute()
//     {
//         string specName = string.Join("", _expression["spec"].ToString().Split('_').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
//
//         string key = _expression.ContainsKey("data") ? "data" : "field";
//         object value;
//     
//         if (!_expression.ContainsKey(key))
//         {
//             _expression.Remove("spec");
//             value = _expression;
//         }
//         else
//         {
//             value = _expression[key];
//         }
//
//         if (specs != null && specs.ContainsKey(specName))
//         {
//             return _builder.Spec(specs[specName], value);
//         }
//
//         return null;
//     }
// }