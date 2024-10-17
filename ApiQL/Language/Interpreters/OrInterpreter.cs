using System.Text.Json;

namespace ApiQL.Language.Interpreters;

internal class OrInterpreter : AbstractLanguage
{
    public OrInterpreter(JsonElement expression) : base(expression)
    { }

    public override object Execute()
    {
        /*
        var expressions = _expression.Select(@params =>
        {
            var dict = new Dictionary<string, object>
            {
                { @params.Key, @params.Value }
            };
            var interpreter = LanguageFactory.Build(dict);
            return interpreter.Execute();
        }).ToArray();

        return _builder.OrX(expressions);
        */

        return null;
    }
}