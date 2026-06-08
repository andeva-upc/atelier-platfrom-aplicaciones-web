using atelier_platform_aplicaciones_web.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.QueryServices;

using atelier_platform_aplicaciones_web.Shared.Resources;
using atelier_platform_aplicaciones_web.Shared.Interfaces.Rest.ProblemDetails;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;

// IoT usings
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Operations Repositories and Services
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IWorkOrderCommandService, WorkOrderCommandService>();
builder.Services.AddScoped<IWorkOrderQueryService, WorkOrderQueryService>();

// IoT Repositories
builder.Services.AddScoped<IOBD2DeviceRepository, OBD2DeviceRepository>();
builder.Services.AddScoped<IOBD2DeviceRegistrationRepository, OBD2DeviceRegistrationRepository>();
builder.Services.AddScoped<ITelemetrySnapshotRepository, TelemetrySnapshotRepository>();
builder.Services.AddScoped<IDtcAlertRepository, DtcAlertRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleRegistrationRepository, VehicleRegistrationRepository>();

// IoT Services
builder.Services.AddScoped<IOBD2DeviceCommandService, OBD2DeviceCommandService>();
builder.Services.AddScoped<IOBD2DeviceQueryService, OBD2DeviceQueryService>();
builder.Services.AddScoped<ITelemetryCommandService, TelemetryCommandService>();
builder.Services.AddScoped<ITelemetryQueryService, TelemetryQueryService>();
builder.Services.AddScoped<IDtcCommandService, DtcCommandService>();
builder.Services.AddScoped<IDtcQueryService, DtcQueryService>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();

// 8. Registro del Custom Problem Details Factory
builder.Services.AddTransient<ProblemDetailsFactory>();

// 9. Cortex.Mediator Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(LoggingCommandBehavior<,>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

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
