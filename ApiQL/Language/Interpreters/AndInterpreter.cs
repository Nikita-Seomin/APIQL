using System.Text.Json;

namespace ApiQL.Language.Interpreters;

/// <summary>
/// Class AndInterpreter
/// </summary>
internal class AndInterpreter : AbstractLanguage
{
    public AndInterpreter(JsonElement expression) : base(expression)
    { }

    /// <summary>
    /// Execute logical AND expression
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
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

        return _builder.AndX(expressions);
        */return null;
    }
}
