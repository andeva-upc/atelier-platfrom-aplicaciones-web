using atelier_platform_aplicaciones_web.Shared.Infrastructure.Interfaces.ASP.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Events;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EFC.Repositories;
using atelier_platform_aplicaciones_web.Operations.Application.Services;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Interfaces.Events;
using atelier_platform_aplicaciones_web.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de URLs en minúsculas y convención Kebab-Case
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// 2. Configuración de Localización (Multi-idioma)
builder.Services.AddLocalization();

// 3. Configuración de Controladores y Validación de Datos Localizada
builder.Services.AddControllers(options => 
    options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

// 4. Registro de ProblemDetails para control de excepciones centralizado (RFC 7807)
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails.Status is null or >= 500)
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.ProblemDetails.Title ??= localizer["UnexpectedServerError"].Value;
            context.ProblemDetails.Detail ??= localizer["UnexpectedErrorProcessingRequest"].Value;
        }
    };
});

// 5. Configuración de Swagger/OpenAPI con anotaciones enriquecidas
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

// 6. Registro de DbContext utilizando PostgreSQL (Npgsql)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");
    
    options.UseNpgsql(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// 7. REGISTRO DE DEPENDENCIAS (Inyección de Dependencias)
// Aquí registraremos los repositorios y servicios de Operations cuando los creemos
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IWorkOrderCommandService, WorkOrderCommandService>();
builder.Services.AddScoped<IWorkOrderQueryService, WorkOrderQueryService>();

builder.Services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
builder.Services.AddScoped<IDomainEventHandler<PaymentProcessedEvent>, WorkOrderPaymentListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Swagger UI habilitado para pruebas exploratorias de tu API
app.UseSwagger();
app.UseSwaggerUI();

// 8. Opciones de Localización en Español e Inglés
string[] supportedCultures = ["es-PE", "es", "en-US", "en"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseAuthorization();

// 9. Mapeo de Controladores de API
app.MapControllers();

app.Run();