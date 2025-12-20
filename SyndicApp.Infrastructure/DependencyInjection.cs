using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SyndicApp.Application.Interfaces;
using SyndicApp.Application.Interfaces.AppelVocal;
using SyndicApp.Application.Interfaces.Communication;
using SyndicApp.Application.Interfaces.Residences;
using SyndicApp.Infrastructure;
using SyndicApp.Infrastructure.Files;
using SyndicApp.Infrastructure.Identity;
using SyndicApp.Infrastructure.Services;
using SyndicApp.Infrastructure.Services.AppelVocal;
using SyndicApp.Infrastructure.Services.Communication;
using SyndicApp.Infrastructure.Services.Residences;
using System.Text;

namespace SyndicApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1️⃣ DbContext + MigrationsAssembly explicite
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("SyndicApp.Infrastructure") // 👈 Corrige le problème EF
                ));

            // 2️⃣ Identity (GUID comme clé)
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 3️⃣ JWT Settings
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            // 4️⃣ Authentication JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // (true en prod)
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ICallService, CallService>();
            // 5️⃣ Services transverses et métiers
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IResidenceService, ResidenceService>();
            services.AddScoped<IBatimentService, BatimentService>();
            services.AddScoped<ILotService, LotService>();
            services.AddScoped<IAffectationLotService, AffectationLotService>();
            services.AddScoped<ILocataireTemporaireService, LocataireTemporaireService>();

            // Tu pourras aussi injecter ici tes services OTP / Email, si besoin.
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IAudioStorage, LocalAudioStorage>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IImageMessageService, ImageMessageService>();
            services.AddScoped<IDocumentMessageService, DocumentMessageService>();
            services.AddScoped<ILocationMessageService, LocationMessageService>();

            return services;
        }
    }
}
