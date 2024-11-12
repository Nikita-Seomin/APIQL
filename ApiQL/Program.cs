using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Nodes;
using ApiQL;
using ApiQL.Language;
using SqlKata;
using SqlKata.Compilers;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddControllers(); // Добавляем поддержку контроллеров

// Добавьте здесь регистрацию других необходимых сервисов, например, для работы с БД или другим функционалом

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Маршрутизируем запросы к контроллерам
});

app.Run();