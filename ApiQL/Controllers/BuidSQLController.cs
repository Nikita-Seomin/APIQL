using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes; // Нужно для использования JsonObject и JsonValue
using ApiQL;
using ApiQL.Language;
using SqlKata;
using SqlKata.Compilers; 

namespace ApiQL.Controllers
{
    [ApiController]
    [Route("api")]
    public class QueryController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetSqlQuery([FromBody] JsonObject request)
        {
            try
            {
                // Создайте начальный запрос к базе данных
                var qb = new Query("users as u")           // убрать from ???
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
