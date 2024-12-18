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
  
  
  //Тест null
  [Fact]
  public void testNull()
  {
   // Простое is_null
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    ["is_null"] = JsonValue.Create("surname")
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NULL", sql);
   Assert.Empty(parameters);
   
   
   // Простое is_not_null
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["is_not_null"] = JsonValue.Create("surname")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NOT NULL", sql);
   Assert.Empty(parameters);
  }
  
  
  //Тест null and Equals
  [Fact]
  public void testNullAndEquals()
  {
   // Простое is_null and Equals
   var qb = _db.Query("users as u")
    .Select("u.id");

   var query = new JsonObject
   {
    ["is_null"] = JsonValue.Create("surname"),
    ["name"] = JsonValue.Create("Ivan")
   };
   var api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   var compiler = new PostgresCompiler();
   SqlResult result = compiler.Compile(qb);
   string sql = result.Sql;
   var parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NULL AND \"name\" = @p0", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
   
   
   // Простое is_not_null and Equals
   qb = _db.Query("users as u")
    .Select("u.id");

   query = new JsonObject
   {
    ["is_not_null"] = JsonValue.Create("surname"),
    ["name"] = JsonValue.Create("Ivan")
   };
   api = new ApiQueryLanguage(query, qb);
   api.Execute();

   // Генерация строки SQL из запроса
   compiler = new PostgresCompiler();
   result = compiler.Compile(qb);
   sql = result.Sql;
   parameters = result.NamedBindings;

   Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NOT NULL AND \"name\" = @p0", sql);
   Assert.Single(parameters);
   Assert.Equal("Ivan", parameters["@p0"].ToString());
  }
  
  
  //Тест Equals and null
   [Fact]
   public void testEqualsAndNull()
   {
    // Простое Equals and is_null 
    var qb = _db.Query("users as u")
     .Select("u.id");
  
    var query = new JsonObject
    {
     ["name"] = JsonValue.Create("Ivan"),
     ["is_null"] = JsonValue.Create("surname")
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();
  
    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;
  
    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"name\" = @p0 AND \"surname\" IS NULL", sql);
    Assert.Single(parameters);
    Assert.Equal("Ivan", parameters["@p0"].ToString());
    
    
    // Простое Equals and is_not_null 
    qb = _db.Query("users as u")
     .Select("u.id");
  
    query = new JsonObject
    {
     ["name"] = JsonValue.Create("Ivan"),
     ["is_not_null"] = JsonValue.Create("surname")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();
  
    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;
  
    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"name\" = @p0 AND \"surname\" IS NOT NULL", sql);
    Assert.Single(parameters);
    Assert.Equal("Ivan", parameters["@p0"].ToString());
   }
   
   
   //Тест spec null
   [Fact]
   public void testNullSpec()
   {
    // Простое spec null
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["spec"] = JsonValue.Create("IsNull"),
     ["field"] = JsonValue.Create("surname")
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NULL", sql);
    Assert.Empty(parameters);
    
    
    // Простое spec null lower_case reverse
     qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["field"] = JsonValue.Create("surname"),
     ["spec"] = JsonValue.Create("is_null")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NULL", sql);
    Assert.Empty(parameters);
    
    
    // spec IsNotNull
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("IsNotNull"),
     ["field"] = JsonValue.Create("surname")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NOT NULL", sql);
    Assert.Empty(parameters);
    
    
    // spec is_not_null lower_case reverse
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["field"] = JsonValue.Create("surname"),
     ["spec"] = JsonValue.Create("is_not_null")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE \"surname\" IS NOT NULL", sql);
    Assert.Empty(parameters);
   }
   
   
   // like
   [Fact]
   public void testLike()
   {
    // like
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["like"] = new JsonObject
      {
       ["name"] = JsonValue.Create("John")
      }
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" ilike @p0 ESCAPE \'*\')", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    // or [like, like]
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["or"] = new JsonArray
     {
      new JsonObject
      {
       ["like"] = new JsonObject
       {
        ["name"] = JsonValue.Create("John")
       }
      },
      new JsonObject
      {
       ["like"] = new JsonObject
       {
        ["name"] = JsonValue.Create("Smith")
       }
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

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" ilike @p0 ESCAPE \'*\') OR (\"name1\" ilike @p1 ESCAPE \'*\')", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Smith", parameters["@p1"].ToString());
    
    
    
    // not_like
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["not_like"] = new JsonObject
     {
      ["name"] = JsonValue.Create("John")
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" ilike @p0 ESCAPE \'*\'))", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    
    // rel like
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
      ["rel"] = JsonValue.Create("like"),
      ["name"] = JsonValue.Create("John")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" ilike @p0 ESCAPE \'*\')", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    
    // rel not_like
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["rel"] = JsonValue.Create("not_like"),
     ["name"] = JsonValue.Create("John")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" ilike @p0 ESCAPE \'*\'))", sql);
    Assert.Single(parameters);
   }
   
   // spec like
   [Fact]
   public void testLikeSpec()
   {
    //spec like
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["name"] = JsonValue.Create("John"),
     ["spec"] = JsonValue.Create("Like")
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" ilike @p0)", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    
    //spec like reverse
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("Like"),
     ["name"] = JsonValue.Create("John")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" ilike @p0)", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    
    //spec not_like
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["name"] = JsonValue.Create("John"),
     ["spec"] = JsonValue.Create("NotLike")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" ilike @p0))", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
    
    
    //spec not_like lower_case reverse
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("not_like"),
     ["name"] = JsonValue.Create("John")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (NOT (\"name\" ilike @p0))", sql);
    Assert.Single(parameters);
    Assert.Equal("John", parameters["@p0"].ToString());
   }
   
   
   // in
   [Fact]
   public void testIn()
   {
    //in
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["in"] = new JsonObject
     {
      ["name"] = new JsonArray
      {
       JsonValue.Create("John"),
       JsonValue.Create("Ivan")
      }
     }
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    // rel in
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["rel"] = JsonValue.Create("in"),
      ["name"] = new JsonArray
      {
       JsonValue.Create("John"),
       JsonValue.Create("Ivan")
      }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    // not_in
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["not_in"] = new JsonObject
     {
      ["name"] = new JsonArray
      {
       JsonValue.Create("John"),
       JsonValue.Create("Ivan")
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

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" NOT IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    // rel not_in
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["rel"] = JsonValue.Create("not_in"),
     ["name"] = new JsonArray
     {
      JsonValue.Create("John"),
      JsonValue.Create("Ivan")
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" NOT IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
   }
   
   
   // in
   [Fact]
   public void TestInSpec()
   {
    //in
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["name"] = new JsonArray
     {
      JsonValue.Create("John"),
      JsonValue.Create("Ivan")
     },
     ["spec"] = JsonValue.Create("In")
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    //in reverse
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("In"),
     ["name"] = new JsonArray
     {
      JsonValue.Create("John"),
      JsonValue.Create("Ivan")
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    // not_in
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["name"] = new JsonArray
     {
      JsonValue.Create("John"),
      JsonValue.Create("Ivan")
     },
     ["spec"] = JsonValue.Create("NotIn")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" NOT IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
    
    
    // not_in lower_case reverse
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("NotIn"),
     ["name"] = new JsonArray
     {
      JsonValue.Create("John"),
      JsonValue.Create("Ivan")
     }
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"name\" NOT IN (@p0, @p1))", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("John", parameters["@p0"].ToString());
    Assert.Equal("Ivan", parameters["@p1"].ToString());
   }
   
   
   // spec additional
   [Fact]
   public void TestAdditionalSpecs()
   {
    //spec Between []
    var qb = _db.Query("users as u")
     .Select("u.id");

    var query = new JsonObject
    {
     ["spec"] = JsonValue.Create("Between"),
     ["age"] = new JsonArray
     {
      JsonValue.Create(10),
      JsonValue.Create(20)
     },
    };
    var api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    var compiler = new PostgresCompiler();
    SqlResult result = compiler.Compile(qb);
    string sql = result.Sql;
    var parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"age\" BETWEEN @p0 AND @p1)", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal(10, parameters["@p0"]);
    Assert.Equal(20, parameters["@p1"]);
    
    
    // spec Between str
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("Between"),
     ["age"] = JsonValue.Create("10,20")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"age\" BETWEEN @p0 AND @p1)", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal(10, parameters["@p0"]);
    Assert.Equal(20, parameters["@p1"]);
    
    
    // spec between []
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("between"),
     ["age"] = new JsonArray
     {
      JsonValue.Create(10),
      JsonValue.Create(20)
     },
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"age\" BETWEEN @p0 AND @p1)", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal(10, parameters["@p0"]);
    Assert.Equal(20, parameters["@p1"]);
    
    
    // spec between []
    qb = _db.Query("users as u")
     .Select("u.id");

    query = new JsonObject
    {
     ["spec"] = JsonValue.Create("between"),
     ["age"] = JsonValue.Create("10,20")
    };
    api = new ApiQueryLanguage(query, qb);
    api.Execute();

    // Генерация строки SQL из запроса
    compiler = new PostgresCompiler();
    result = compiler.Compile(qb);
    sql = result.Sql;
    parameters = result.NamedBindings;

    Assert.Equal("SELECT \"u\".\"id\" FROM \"users\" AS \"u\" WHERE (\"age\" BETWEEN @p0 AND @p1)", sql);
    Assert.Equal(2, parameters.Count);
    Assert.Equal(10, parameters["@p0"]);
    Assert.Equal(20, parameters["@p1"]);
   }
 }
