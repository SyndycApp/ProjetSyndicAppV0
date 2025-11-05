using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

using SyndicApp.Application.Interfaces;                 // IEmailSender, IPasswordService
using SyndicApp.Application.Interfaces.Finances;
using SyndicApp.Application.Interfaces.Incidents;
using SyndicApp.Application.Interfaces.Residences;

using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Data;                   // SeedData
using SyndicApp.Infrastructure.Identity;               // JwtSettings, RolesSeeder
using SyndicApp.Infrastructure.Services;               // SmtpEmailSender, PasswordService
using SyndicApp.Infrastructure.Services.Finances;
using SyndicApp.Infrastructure.Services.Incidents;
using SyndicApp.Infrastructure.Services.Residences;

var builder = WebApplication.CreateBuilder(args);

// ================== CONFIG ==================
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // enums lisibles
    });

// CORS (2 policies pratiques en dev)
builder.Services.AddCors(o =>
{
    // Autorise n'importe quelle origine (DEV rapide : web, mobile, etc.)
    o.AddPolicy("DevAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    // Exemple d’origine précise (si tu veux restreindre)
    o.AddPolicy("LocalDev", p => p
        .WithOrigins("https://localhost:7263", "http://localhost:7263")
        .AllowAnyHeader().AllowAnyMethod());
});

// ========== Swagger (avec bouton Authorize JWT) ==========
builder.Services.AddEndpointsApiExplorer();
// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SyndicApp API",
        Version = "v1",
        Description = "API de gestion pour l'application SyndicApp"
    });

    // >>> remplace l'ancienne définition ApiKey par celle-ci :
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT auth avec le schéma Bearer.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,   // << Http (pas ApiKey)
        Scheme = "Bearer",                // << Swagger va préfixer automatiquement
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// ================== INFRASTRUCTURE ==================
// ⚠️ AddInfrastructure configure déjà DbContext + Identity (+ potentiellement JWT).
// Ne pas reconfigurer AddAuthentication ici pour éviter "Scheme already exists: Bearer".
builder.Services.AddInfrastructure(builder.Configuration);

// Identity (dev-friendly)
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 6;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireLowercase = false;
});

// Services transverses & métiers
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddTransient<IPasswordService, PasswordService>();

builder.Services.AddTransient<IAffectationLotService, AffectationLotService>();
builder.Services.AddTransient<IResidenceService, ResidenceService>();
builder.Services.AddTransient<ILotService, LotService>();
builder.Services.AddTransient<IBatimentService, BatimentService>();
builder.Services.AddTransient<ILocataireTemporaireService, LocataireTemporaireService>();

builder.Services.AddScoped<IChargeService, ChargeService>();
builder.Services.AddScoped<IAppelDeFondsService, AppelDeFondsService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();
builder.Services.AddScoped<ISoldeService, SoldeService>();

// ✅ correction de syntaxe ici :
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IDevisTravauxService, DevisTravauxService>();
builder.Services.AddScoped<IInterventionService, InterventionService>();

// DataProtection (persistance des clés)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")));

// AutoMapper (si profils dans Infrastructure)
builder.Services.AddAutoMapper(typeof(SyndicApp.Infrastructure.Services.Mapping.ResidenceProfile).Assembly);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// ================== BUILD ==================
var app = builder.Build();

// ================== SEEDING (après Build, avant Middleware) ==================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await RolesSeeder.SeedAsync(roleManager, logger);
    await SeedData.Initialize(services);
}

// ================== PIPELINE HTTP ==================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyndicApp API v1");
        c.RoutePrefix = string.Empty; // Swagger sur /
    });

    // En dev, on autorise tout pour simplifier les tests (web + mobile)
    app.UseCors("DevAll");
}
else
{
    // En prod, restreins (ex: LocalDev remplacé par tes domaines publics)
    app.UseCors("LocalDev");
}

//app.UseHttpsRedirection();

app.UseAuthentication();   // ← gardé (configuré dans l’infrastructure)
app.UseAuthorization();

app.MapControllers();

app.Run();
