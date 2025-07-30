using Microsoft.OpenApi.Models;
using SyndicApp.Infrastructure;
using System.Text.Json.Serialization;
using SyndicApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using SyndicApp.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Ajout de la config JWT depuis appsettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Ajouter les services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Swagger
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

// 🔐 Ajout d'Identity + JWT + DbContext
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await RolesSeeder.SeedAsync(roleManager, logger);
    await SeedData.Initialize(services);
}
// Swagger en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyndicApp API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 🔐 N'oublie pas ça
app.UseAuthorization();

app.MapControllers();

app.Run();
