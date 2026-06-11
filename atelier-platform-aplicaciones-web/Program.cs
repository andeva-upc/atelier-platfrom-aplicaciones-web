using atelier_platform_aplicaciones_web.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.Internal.QueryServices;

using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.Billing.Application.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Billing.Application.QueryServices;
using atelier_platform_aplicaciones_web.Billing.Application.Internal.QueryServices;

using atelier_platform_aplicaciones_web.IAM.Application.CommandServices;
using atelier_platform_aplicaciones_web.IAM.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.IAM.Application.Internal.OutboundServices.Email;
using atelier_platform_aplicaciones_web.IAM.Application.Internal.OutboundServices.Hashing;
using atelier_platform_aplicaciones_web.IAM.Application.Internal.OutboundServices.Tokens;
using atelier_platform_aplicaciones_web.IAM.Application.Internal.QueryServices;
using atelier_platform_aplicaciones_web.IAM.Application.QueryServices;
using atelier_platform_aplicaciones_web.IAM.Domain.Repositories;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Email.Smtp;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Hashing.BCrypt;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Tokens.Jwt.Configuration;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Tokens.Jwt.Services;

using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Application.Internal.CommandServices;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Application.Internal.QueryServices;

using atelier_platform_aplicaciones_web.Shared.Resources;
using atelier_platform_aplicaciones_web.Shared.Interfaces.Rest.ProblemDetails;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de URLs en minúsculas y convención Kebab-Case
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// 2. Configuración de Localización (Multi-idioma)
builder.Services.AddLocalization();

// 3. Configuración de Controladores y Validación de Datos Localizada
builder.Services.AddControllers(options => 
    options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => 
            factory.Create(typeof(SharedResource));
    });

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
builder.Services.AddSwaggerGen(options => 
{
    options.EnableAnnotations();
    
    // Configuración de Seguridad para Swagger (Botón Authorize)
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    
    options.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// 6. Configuración de dependencias de infraestructura compartida
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.AuditableEntityInterceptor>();
builder.Services.AddScoped<atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors.DispatchDomainEventsInterceptor>();

// 7. Registro de DbContext utilizando PostgreSQL (Npgsql)
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

// IAM Dependencies
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordRecoveryTokenRepository, PasswordRecoveryTokenRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IPasswordRecoveryCommandService, PasswordRecoveryCommandService>();

// Core Services
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchSubscriptionRepository, BranchSubscriptionRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();

builder.Services.AddScoped<IBranchCommandService, BranchCommandService>();
builder.Services.AddScoped<ICustomerCommandService, CustomerCommandService>();
builder.Services.AddScoped<IEmployeeCommandService, EmployeeCommandService>();
builder.Services.AddScoped<IOwnerCommandService, OwnerCommandService>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<IWorkshopCommandService, WorkshopCommandService>();

builder.Services.AddScoped<IBranchQueryService, BranchQueryService>();
builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();
builder.Services.AddScoped<IEmployeeQueryService, EmployeeQueryService>();
builder.Services.AddScoped<IOwnerQueryService, OwnerQueryService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IWorkshopQueryService, WorkshopQueryService>();

builder.Services.AddScoped<IHashingService, BCryptHashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

// Billing Dependencies
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IQuoteCommandService, QuoteCommandService>();
builder.Services.AddScoped<IQuoteQueryService, QuoteQueryService>();

// TokenSettings Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// JWT Configuration
var secret = builder.Configuration["TokenSettings:Secret"];
if (string.IsNullOrEmpty(secret))
    throw new InvalidOperationException("JWT Secret is not configured.");
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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

app.UseRequestAuthorization();

// 9. Mapeo de Controladores de API
app.MapControllers();

app.Run();
