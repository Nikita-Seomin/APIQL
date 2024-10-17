 using System.Text.Json.Nodes;

 namespace tests;
//
// public class Tests
// {
//     [SetUp]
//     public void Setup()
//     {
//     }
//
//     [Test]
//     public void Test1()
//     {
//         Assert.Pass();
//     }
// }

 using System.Collections.Generic;
 using Xunit;
 using SqlKata;
 using SqlKata.Execution;
 using SqlKata.Compilers;
 using Npgsql; // NuGet пакет Npgsql для работы с PostgreSQL
 using Moq;
 using ApiQL;

 public class ApiQlUnitTestSeparate
 {
  private IDbConnectionWrapper _connMock;
  private QueryFactory _db;

  public ApiQlUnitTestSeparate()
  {
   string connectionString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase"; // Замените на ваши реальные данные
   _connMock = new NpgsqlConnectionWrapper(connectionString);
   _db = new QueryFactory(_connMock, new SqlKata.Compilers.PostgresCompiler());
  }

  //Тест простейшего равенства
  [Fact]
  public void TestEquality()
  {
   // Простое равенство
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    { "name", "Ivan" }
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"name\" = @p0", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
   
   
   //Равенство с eq
   qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["eq"] = new JsonObject
     {
      ["name"] = JsonValue.Create("Ivan")
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"name\" = @p0", sql);
    Assert.Single(parameters);
    Assert.Equal("Ivan", parameters["@p0"].ToString());
    
    
    // Равенство с where и eq
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["where"] = new JsonObject
     {
      ["eq"] = new JsonObject
      {
       ["name"] = JsonValue.Create("Ivan")
      } 
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"name\" = @p0", sql);
    Assert.Single(parameters);
    Assert.Equal("Ivan", parameters["@p0"].ToString());
    
    
    //Неравенство с neq
    qb = _db.Query("users as u")
     .Select("u.id");
    
    query = new JsonObject
    {
     ["where"] = new JsonObject
     {
      ["neq"] = new JsonObject
      {
       ["name"] = JsonValue.Create("Ivan")
      }
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();
    
    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;
    
    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" = @p0) OR \"name\" IS NULL)", sql);
    Assert.Single(parameters);
    Assert.Equal("Ivan", parameters["@p0"].ToString());
  }

  [Fact]
  public void TestTest()
  {
   // Простое равенство
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    ["neq"] = new JsonObject
    {
     ["name"] = JsonValue.Create("Ivan")
    }
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" = @p0) OR \"name\" IS NULL)", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
  }

 }
