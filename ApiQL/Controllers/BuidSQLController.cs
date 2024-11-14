using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes; // Нужно для использования JsonObject и JsonValue
using ApiQL;
using ApiQL.Language;
using SqlKata;
using SqlKata.Compilers; 

namespace ApiQL.Controllers
{
    [ApiController]
    [Route("mapeditor/geojson")]
    public class QueryController : ControllerBase
    {
        [HttpPost]
        [Route("{scheme}/{tableName}")] // передали схему и таблицу 
        public IActionResult GetSqlQuery([FromRoute] string scheme, [FromRoute] string tableName,[FromBody] JsonObject request)
        { 
            try
            {
                // Извлечение параметров запроса
                var cqlFilter = Request.Query["cql_filter"].ToString();
                var srsName = Request.Query["srsname"].ToString();
                
                if (scheme == "registry" && int.TryParse(tableName, out var intTableName)) // если схема registry и tableName хранит int id то обернуть int в object_{id}_
                    tableName = "object_" + tableName + "_";
                
                // Создайте начальный запрос к базе данных
                var qb = new Query(scheme + "." + tableName)           // убрать from ???
                     .Select("u.id");                                   // убрать как появится логика select

                // Создайте экземпляр ApiQueryLanguage с JSON-запросом
                var api = new ApiQueryLanguage(request, qb);
                api.Execute();

                // Генерация SQL-строки
                var compiler = new PostgresCompiler();
                var result = compiler.Compile(qb);
                var sql = result.Sql;

                // Возвращаем SQL-строку как результат
                return Ok(new { sql = sql, parameters = result.NamedBindings });
            }
            catch (Exception ex)
            {
                // Обработка исключений и возврат ошибки
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
