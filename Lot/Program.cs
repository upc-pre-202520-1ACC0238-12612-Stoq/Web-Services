using AyniTech.IAM.Application.QueryServices;
using Lot.IAM.Application.CommandServices;
using Lot.IAM.Application.OutBoundServices;
using Lot.IAM.Domain.Repositories;
using Lot.IAM.Domain.Services;
using Lot.IAM.Infrastructure.Hashing.BCrypt.Services;
using Lot.IAM.Infrastructure.Repositories;
using Lot.IAM.Infrastructure.Tokens.JWT.Configuration;
using Lot.IAM.Infrastructure.Tokens.JWT.Services;
using Lot.Inventaries.Application.Internal.CommandServices;
using Lot.Inventaries.Application.Internal.QuerysServices;
using Lot.Inventaries.Domain.Repositories;
using Lot.Inventaries.Domain.Services;
using Lot.Inventaries.Infraestructure.Persistence.EFC.Repositories;

using Lot.ProductManagement.Application.Internal.CommandServices;
using Lot.ProductManagement.Application.Internal.QueryServices;
using Lot.ProductManagement.Domain.Repositories;
using Lot.ProductManagement.Domain.Services;
using Lot.ProductManagement.Infrastructure.Persistence.EFC.Repositories;
using Lot.Shared.Domain.Repositories;
using Lot.Shared.Infrastructure.Persistence.EFC.Seeding;
using Lot.Shared.Infraestructure.ASP.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Lot.Shared.Infraestructure.Persistence.EFC.Repositories;
using Lot.Reports.Domain.Repositories;
using Lot.Reports.Domain.Services;
using Lot.Reports.Application.Internal.CommandServices;
using Lot.Reports.Infrastructure.Persistence.EFC.Repositories;
using Lot.Reports.Application.Internal.QueryServices;

using Lot.Sales.Domain.Repositories;
using Lot.Sales.Domain.Services;
using Lot.Sales.Application.Internal.CommandServices;
using Lot.Sales.Infrastructure.Persistence.EFC.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Lot.AlertStockManagement.Application.Internal.QueryServices;
using Lot.AlertStockManagement.Domain.Repositories;
using Lot.AlertStockManagement.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure JWT Authentication
var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>();
var key = Encoding.ASCII.GetBytes(tokenSettings?.Secret ?? "default-secret-key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            Console.WriteLine("🔐 Token validado exitosamente");
            var claims = context.Principal.Claims;
            Console.WriteLine("📜 Claims del token:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"   - {claim.Type}: {claim.Value}");
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("❌ Fallo en la autenticación del token:");
            Console.WriteLine($"   Error: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("🚫 Desafío de autenticación:");
            Console.WriteLine($"   Error: {context.Error}");
            Console.WriteLine($"   Error Description: {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});

// Add Authorization
builder.Services.AddAuthorization();

if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT token — escribe solo el token, el prefijo 'Bearer' se agrega automáticamente.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});



// Dependency Injection

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWorks>();

// IAM Bounded Context Injection Configuration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Inventaries Bounded Context
builder.Services.AddScoped<IInventoryByProductRepository, InventoryByProductRepository>();
builder.Services.AddScoped<IInventoryByBatchRepository, InventoryByBatchRepository>();

builder.Services.AddScoped<IInventoryByProductCommandService, InventoryByProductCommandService>();
builder.Services.AddScoped<IInventoryByBatchCommandService, InventoryByBatchCommandService>();

builder.Services.AddScoped<IInventoryByProductQueryService, InventoryByProductQueryService>();
builder.Services.AddScoped<IInventoryByBatchQueryService, InventoryByBatchQueryService>();

// ProductManagement Bounded Context
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagQueryService, TagQueryService>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUnitQueryService, UnitQueryService>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<IComboRepository, ComboRepository>();
builder.Services.AddScoped<IComboCommandService, ComboCommandService>();
builder.Services.AddScoped<IComboQueryService, ComboQueryService>();

// Registrar repositorios
builder.Services.AddScoped<IComboRepository, ComboRepository>();

// Registrar servicios
builder.Services.AddScoped<IComboCommandService, ComboCommandService>();
builder.Services.AddScoped<IComboQueryService, ComboQueryService>();

// Reports Bounded Context
builder.Services.AddScoped<IStockAverageReportCommandService, StockAverageReportCommandService>();
builder.Services.AddScoped<ICategoryReportCommandService, CategoryReportCommandService>();
builder.Services.AddScoped<IStockAverageReportQueryService, StockAverageReportQueryService>();
builder.Services.AddScoped<ICategoryReportQueryService, CategoryReportQueryService>();
builder.Services.AddScoped<IStockAverageReportRepository, StockAverageReportRepository>();
builder.Services.AddScoped<ICategoryReportRepository, CategoryReportRepository>();

// Sales Bounded Context
builder.Services.AddScoped<ISaleCommandService, SaleCommandService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// AlertStockManagement Bounded Context
builder.Services.AddScoped<IInventoryReadRepository, InventoryReadRepository>();
builder.Services.AddScoped<IStockAlertReadRepository, InventoryReadRepository>();
builder.Services.AddScoped<IStockAlertQueryService, StockAlertQueryService>();

Console.WriteLine("Construyendo la aplicación...");
var app = builder.Build();
Console.WriteLine("Aplicación construida exitosamente");

// Verifica si la base de datos existe y créala si no existe
Console.WriteLine("Inicializando base de datos...");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Crear la base de datos si no existe
    Console.WriteLine("Creando base de datos si no existe...");
    context.Database.EnsureCreated();
    Console.WriteLine("Base de datos inicializada correctamente");

    try
    {
        Console.WriteLine("Iniciando seeding de datos de ejemplo...");
        await DataSeederService.SeedDataAsync(context);
        Console.WriteLine("Seeding de datos completado exitosamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error durante el seeding de datos: {ex.Message}");
        Console.WriteLine($"Detalles: {ex.InnerException?.Message}");
    }
}

// Configure the HTTP request pipeline.

// En desarrollo SIEMPRE; en producción solo si Swagger__Enabled=true
var enableSwagger = builder.Configuration.GetValue<bool>("Swagger:Enabled", false);
if (app.Environment.IsDevelopment() || enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lot API v1");
        c.RoutePrefix = "swagger"; // URL: /swagger
    });
}

app.UseCors("AllowAllPolicy");

// Mantengo tu redirección HTTPS tal cual la tenías.
// (Si más adelante pones proxy TLS, podemos mover esta parte.)
// Redirige a HTTPS solo en desarrollo (en contenedor usamos HTTP :8080)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Importante: UseAuthentication debe ir ANTES de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
