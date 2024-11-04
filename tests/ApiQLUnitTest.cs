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
    
    
    //Неравенство с neq и where
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
    
    
    //Равенство с eq и rel
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
      ["name"] = JsonValue.Create("Ivan"),
      ["rel"] = JsonValue.Create("eq")
      
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
    
    
    //Равенство с rel и eq
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["rel"] = JsonValue.Create("eq"),
     ["name"] = JsonValue.Create("Ivan")
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
    
    //Неавенство с eq и rel
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["name"] = JsonValue.Create("Ivan"),
     ["rel"] = JsonValue.Create("neq")
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
  public void testEqualsSpecs()
  {
   // Простое равенство
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    { "name", "Ivan" },
    { "spec", "Equals" }
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
   
   
   // Простое равенство наоборот
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    { "spec", "Equals" },
    { "name", "Ivan" }
    
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
   
   
   
   // Простое равенство lower
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    { "name", "Ivan" },
    { "spec", "equals" }
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
   
   
   
   // Простое равенство наоборот lower
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    { "spec", "equals" },
    { "name", "Ivan" }
    
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
   
   
   // Простое неравенство
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    { "name", "Ivan" },
    { "spec", "NotEquals" }
    
    
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" = @p0))", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
   
   
   // Простое неравенство наоборот
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    { "spec", "NotEquals" },
    { "name", "Ivan" }
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" = @p0))", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
   
   
   // Простое неравенство наоборот c _
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    
    { "spec", "not_equals" },
    { "name", "Ivan" }
    
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" = @p0))", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
   
  }

  //Тест больше чем
  [Fact]
  public void testGreaterLess()
  {
   // Простое >
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    ["gt"] = new JsonObject
    {
     ["age"] = JsonValue.Create(20)
    }
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" > @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   // Простое >=
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["gte"] = new JsonObject
    {
     ["age"] = JsonValue.Create(20)
    }
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" >= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   // rel >
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
     ["age"] = JsonValue.Create(20),
     ["rel"] = JsonValue.Create("gt")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" > @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // rel >
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["rel"] = JsonValue.Create("gte")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" >= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // Простое <
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["lt"] = new JsonObject
    {
     ["age"] = JsonValue.Create(20)
    }
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" < @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // Простое <=
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["lte"] = new JsonObject
    {
     ["age"] = JsonValue.Create(20)
    }
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" <= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // rel <
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["rel"] = JsonValue.Create("lt")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" < @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // rel <
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["rel"] = JsonValue.Create("lte")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" <= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
  }

 //Тест < > spec
  [Fact]
  public void testGreaterLessSpecs()
  {
   // spec >
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
     ["age"] = JsonValue.Create(20),
     ["spec"] = JsonValue.Create("GreaterThan")
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" > @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
  
   
   // spec > lower_case
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("greater_than")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" > @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec > lower_case reverse
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["spec"] = JsonValue.Create("greater_than"),
    ["age"] = JsonValue.Create(20)
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" > @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec >=
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("GreaterThanOrEqual")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" >= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec >= lower_case
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("greater_than_or_equal")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" >= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec >= lower_case reverse
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["spec"] = JsonValue.Create("greater_than_or_equal"),
    ["age"] = JsonValue.Create(20)
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" >= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec <
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("LessThan"),
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" < @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec < lower_case
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("less_than")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" < @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec < lower_case reverse
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["spec"] = JsonValue.Create("less_than"),
    ["age"] = JsonValue.Create(20)
    
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" < @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec <=
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("LessThanOrEqual")
    
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" <= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec <= lower_case
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["age"] = JsonValue.Create(20),
    ["spec"] = JsonValue.Create("less_than_or_equal")
    
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" <= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
   
   
   // spec <= lower_case reverse
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["spec"] = JsonValue.Create("less_than_or_equal"),
    ["age"] = JsonValue.Create(20)
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"age\" <= @p0", sql);
   Assert.Single(parameters);
   Assert.Equal((long)20, parameters["@p0"]);
  }
 }
