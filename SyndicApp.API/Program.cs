using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using SyndicApp.Application.Interfaces;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Application.Interfaces.Finances;
using SyndicApp.Application.Interfaces.Incidents;
using SyndicApp.Application.Interfaces.Personnel;
using SyndicApp.Application.Interfaces.Residences;

using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Data;
using SyndicApp.Infrastructure.Identity;
using SyndicApp.Infrastructure.Services;
using SyndicApp.Infrastructure.Services.Communication;
using SyndicApp.Infrastructure.Services.Finances;
using SyndicApp.Infrastructure.Services.Incidents;
using SyndicApp.Infrastructure.Services.Personnel;
using SyndicApp.Infrastructure.Services.Residences;

using SyndicApp.API.Hubs; // ✅ pour ChatHub

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ================== CONFIG ==================
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ========== CORS ==========
builder.Services.AddCors(o =>
{
    o.AddPolicy("DevAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    o.AddPolicy("LocalDev", p => p
        .WithOrigins("https://localhost:7263", "http://localhost:7263")
        .AllowAnyHeader().AllowAnyMethod());
});

// ========== Swagger + JWT ==========
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SyndicApp API",
        Version = "v1",
        Description = "API de gestion pour l'application SyndicApp"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT auth avec le schéma Bearer.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
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
builder.Services.AddInfrastructure(builder.Configuration);

// Identity dev-friendly
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

builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IDevisTravauxService, DevisTravauxService>();
builder.Services.AddScoped<IInterventionService, InterventionService>();

builder.Services.AddScoped<IPrestataireService, PrestataireService>();

builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IChatService, ChatService>();



// ========== SignalR temps réel ==========
builder.Services.AddSignalR();  // ✅ ajouté

// DataProtection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(SyndicApp.Infrastructure.Services.Mapping.ResidenceProfile).Assembly);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// ================== BUILD ==================
var app = builder.Build();

// ================== SEEDING ==================
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
        c.RoutePrefix = string.Empty;
    });

    app.UseCors("DevAll");
}
else
{
    app.UseCors("LocalDev");
}

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

// ========== MAPPING DU HUB SIGNALR ==========
app.MapHub<ChatHub>("/chatHub"); // ✅ très important

app.Run();
