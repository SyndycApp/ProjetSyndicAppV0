using Blazored.LocalStorage;
using SyndicApp.Application.Interfaces;
using SyndicApp.Infrastructure.Services;
using SyndicApp.Infrastructure; // pour ApplicationDbContext
using Microsoft.EntityFrameworkCore; // pour UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// Ajout DbContext avec connexion SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Configure HttpClient with base address
builder.Services.AddHttpClient();

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    client.BaseAddress = new Uri("http://localhost:5041/");
    return client;
});

// Injection des services métier
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
