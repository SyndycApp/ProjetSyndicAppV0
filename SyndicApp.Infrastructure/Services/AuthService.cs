using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SyndicApp.Application.DTOs.Auth;
using SyndicApp.Application.Interfaces;
using SyndicApp.Domain.Entities.Personnel;
using SyndicApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtTokenGenerator _jwt;
        private readonly ApplicationDbContext _db;   // 🔹 nouveau

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IJwtTokenGenerator jwt,
            ILogger<AuthService> logger,
            ApplicationDbContext db)                
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt;
            _logger = logger;
            _db = db;
        }

        public async Task<Prestataire?> GetPrestataireEntityAsync(Guid id)
        {
            return await _db.Prestataires
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task BindPrestataireToUserAsync(Guid prestataireId, Guid userId)
        {
            var entity = await _db.Prestataires
                                  .FirstOrDefaultAsync(p => p.Id == prestataireId);

            if (entity == null)
                return;

            entity.UserId = userId;
            await _db.SaveChangesAsync();
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto dto)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(dto.Email);
                if (userExists != null)
                    return Result<UserDto>.Fail("Email déjà utilisé");

                if (dto.DateNaissance == null)
                    return Result<UserDto>.Fail("La date de naissance est obligatoire.");

                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    Adresse = dto.Adresse,
                    DateNaissance = dto.DateNaissance.Value
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Erreur création user : {Errors}", string.Join(", ", errors));
                    return Result<UserDto>.Fail(errors);
                }

                if (!string.IsNullOrWhiteSpace(dto.Role))
                {
                    if (!await _roleManager.RoleExistsAsync(dto.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole<Guid>(dto.Role));
                    }

                    await _userManager.AddToRoleAsync(user, dto.Role);
                }
                else
                {
                    return Result<UserDto>.Fail("Le rôle est obligatoire.");
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (string.IsNullOrEmpty(user.Email))
                    return Result<UserDto>.Fail("L'utilisateur n'a pas d'adresse email valide.");

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = _jwt.GenerateToken(
                        new UserDto { Id = user.Id, Email = user.Email, FullName = user.FullName },
                        roles),
                    Roles = roles.ToList()
                };

                return Result<UserDto>.Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de l'inscription");
                return Result<UserDto>.Fail("Erreur interne du serveur");
            }
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Result<UserDto>.Fail("Identifiants invalides.");

                var roles = await _userManager.GetRolesAsync(user);

                if (string.IsNullOrEmpty(user.Email))
                    return Result<UserDto>.Fail("L'utilisateur n'a pas d'adresse email valide.");

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList(),
                    Token = _jwt.GenerateToken(
                        new UserDto
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FullName = user.FullName
                        },
                        roles
                    )
                };

                return Result<UserDto>.Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la connexion");
                return Result<UserDto>.Fail("Erreur interne du serveur");
            }
        }

        public async Task<Result<List<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = _userManager.Users.ToList();

                if (!users.Any())
                    return Result<List<UserDto>>.Fail("Aucun utilisateur trouvé.");

                var result = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    result.Add(new UserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email ?? string.Empty,
                        Roles = roles.ToList()
                    });
                }

                return Result<List<UserDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la récupération des utilisateurs");
                return Result<List<UserDto>>.Fail("Erreur interne du serveur");
            }
        }

        public async Task<Result<List<UserLookupDto>>> SearchAsync(string? q, string? role, int take = 35)
        {
            try
            {
                var queryable = _userManager.Users.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    queryable = queryable.Where(u =>
                        (u.FullName != null && u.FullName.ToLower().Contains(term)) ||
                        (u.Email != null && u.Email.ToLower().Contains(term)));
                }

                queryable = queryable
                    .OrderBy(u => u.FullName ?? u.Email)
                    .ThenBy(u => u.Email);

                var users = await queryable.Take(Math.Max(1, take)).ToListAsync();

                if (!string.IsNullOrWhiteSpace(role))
                {
                    var inRole = await _userManager.GetUsersInRoleAsync(role);
                    var inRoleIds = inRole.Select(x => x.Id).ToHashSet();
                    users = users.Where(u => inRoleIds.Contains(u.Id)).ToList();
                }

                var lookups = new List<UserLookupDto>(users.Count);
                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    lookups.Add(new UserLookupDto
                    {
                        Id = u.Id,
                        Label = !string.IsNullOrWhiteSpace(u.FullName)
                            ? u.FullName!
                            : (u.Email ?? u.Id.ToString()),
                        Email = u.Email,
                        Roles = roles.ToList()
                    });
                }

                return Result<List<UserLookupDto>>.Ok(lookups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du lookup utilisateurs");
                return Result<List<UserLookupDto>>.Fail("Erreur interne du serveur");
            }
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                    return Result<UserDto>.Fail("Utilisateur introuvable.");

                var roles = await _userManager.GetRolesAsync(user);

                if (string.IsNullOrEmpty(user.Email))
                    return Result<UserDto>.Fail("L'utilisateur n'a pas d'adresse email valide.");

                return Result<UserDto>.Ok(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne lors de la récupération de l'utilisateur");
                return Result<UserDto>.Fail("Erreur interne du serveur");
            }
        }
    }
}
