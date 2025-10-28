using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
// ⚠️ Si tu utilises vraiment ces services, vérifie leurs namespaces.
// Sinon, commente les deux lignes d’enregistrement plus bas.
using SyndicApp.Application.Interfaces;       // IEmailSender, IPasswordService (si c’est bien ton namespace)
using SyndicApp.Infrastructure;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Infrastructure.Data;
using SyndicApp.Infrastructure.Identity;
using SyndicApp.Infrastructure.Services;     // SmtpEmailSender, PasswordService (si c’est bien ton namespace)
using System.Text.Json.Serialization;
using SyndicApp.Infrastructure.Services.Residences;
using SyndicApp.Application.Interfaces.Finances;
using Microsoft.EntityFrameworkCore;
using SyndicApp.Infrastructure.Services.Finances;

var builder = WebApplication.CreateBuilder(args);

// === JWT (appsettings.json > "JwtSettings") ===
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// === Controllers & JSON ===
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // éviter les cycles EF → JSON
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
// === CORS (autorise ton front Blazor local) ===
builder.Services.AddCors(o => o.AddPolicy("LocalDev", p =>
    p.WithOrigins("https://localhost:7263") // adapte si ton port change
     .AllowAnyHeader()
     .AllowAnyMethod()
));

// === Swagger ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SyndicApp API",
        Version = "v1",
        Description = "API de gestion pour l'application SyndicApp"
    });
});

// === Infrastructure (DbContext, Identity, AutoMapper, Services métiers, etc.) ===
builder.Services.AddInfrastructure(builder.Configuration);

// === Identity options (dev-friendly) ===
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 6;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireLowercase = false;
});

// === Services transverses (si utilisés dans ton app) ===
// Si les namespaces diffèrent, corrige les using ci-dessus ou commente ces deux lignes.
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

// === DataProtection (persistance des clés pour cookies/tokens) ===
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")));

// === AutoMapper (profils dans l’assembly Infrastructure) ===
builder.Services.AddAutoMapper(typeof(SyndicApp.Infrastructure.Services.Mapping.ResidenceProfile).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(connString)
);

// === Logging console ===
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// === Seed rôles + données de démo ===
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await RolesSeeder.SeedAsync(roleManager, logger);
    await SeedData.Initialize(services);
}

// === Pipeline HTTP ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyndicApp API v1");
        c.RoutePrefix = string.Empty; // Swagger sur /
    });
}

app.UseHttpsRedirection();
app.UseCors("LocalDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
