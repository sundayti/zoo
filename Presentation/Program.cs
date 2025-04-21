using System.Reflection;
using Microsoft.OpenApi.Models;
using Infrastructure.Persistence;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1) Контроллеры и JSON options
builder.Services.AddControllers()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.WriteIndented = true);

// 2) Swagger/OpenAPI с XML-комментариями и атрибуциями
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Zoo API",
        Version     = "v1",
        Description = "Web API для управления зоопарком"
    });

    // Поддержка атрибутов [SwaggerOperation], [SwaggerParameter] и других
    c.EnableAnnotations();

    // Подключаем XML-документацию из сборки Presentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

// 3) DI in-memory репозиториев
builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

// 4) DI application-сервисов
builder.Services.AddScoped<IAnimalTransferService, AnimalTransferService>();
builder.Services.AddScoped<IFeedingOrganizationService, FeedingOrganizationService>();
builder.Services.AddScoped<IStatisticsService, ZooStatisticsService>();

var app = builder.Build();

// 5) Глобальная обработка ошибок
app.UseMiddleware<ErrorHandlingMiddleware>();

// 6) Swagger UI (доступен по корню)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo API v1");
        c.RoutePrefix = string.Empty;
    });
}

// 7) Маршрутизация контроллеров
app.MapControllers();

app.Run();