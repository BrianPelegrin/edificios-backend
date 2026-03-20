using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Mappings;
using ProyectoEdificios.Models.DTO.Users;
using ProyectoEdificios.Models.Entities.Users;
using ProyectoEdificios.Models.Entities.Users;
using ProyectoEdificios.Services.Auth;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoEdificios.Services.Users
{
    public sealed class UserService : IUserService
    {
        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            "admin",
            "viewer"
        };

        private readonly ProyectoEdificiosDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        public UserService(ProyectoEdificiosDbContext context, IPasswordHasherService passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .Select(UserMappings.ToDtoExpression)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(UserMappings.ToDtoExpression)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<(bool Success, string? Error, UserDto? User)> CreateAsync(CreateUserDto request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var role = NormalizeRole(request.Role);

            if (!AllowedRoles.Contains(role))
                return (false, "El rol enviado no es válido.", null);

            if (string.IsNullOrWhiteSpace(request.Password))
                return (false, "La contraseña es obligatoria.", null);

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == email, cancellationToken);

            if (emailExists)
                return (false, "Ya existe un usuario con ese correo.", null);


            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Codigo = string.IsNullOrWhiteSpace(request.Codigo) ? null : request.Codigo.Trim(),
                Nombre = request.Name.Trim(),
                Email = email,
                Clave = hashedPassword.Hash,
                Salt = hashedPassword.Salt,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return (true, null, user.ToDto());
        }

        public async Task<(bool Success, string? Error, UserDto? User)> UpdateAsync(int id, UpdateUserDto request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user is null)
                return (false, "El usuario no existe.", null);

            var email = request.Email.Trim().ToLowerInvariant();
            var role = NormalizeRole(request.Role);

            if (!AllowedRoles.Contains(role))
                return (false, "El rol enviado no es válido.", null);

            var emailInUse = await _context.Users
                .AnyAsync(x => x.Id != id && x.Email == email, cancellationToken);

            if (emailInUse)
                return (false, "Ya existe otro usuario con ese correo.", null);

            user.Codigo = string.IsNullOrWhiteSpace(request.Codigo) ? null : request.Codigo.Trim();
            user.Nombre = request.Name.Trim();
            user.Email = email;
            user.Role = role;

            if (request.UpdatePassword)
            {
                if (string.IsNullOrWhiteSpace(request.Password))
                    return (false, "La nueva contraseña es obligatoria.", null);

                var salt = CrearSalt();
                var hashedPassword = _passwordHasher.HashPassword(request.Password);

                user.Clave = hashedPassword.Hash;
                user.Salt = hashedPassword.Salt;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return (true, null, user.ToDto());
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user is null)
                return (false, "El usuario no existe.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return (true, null);
        }

        private static string NormalizeRole(string? role)
        {
            return string.IsNullOrWhiteSpace(role)
                ? "viewer"
                : role.Trim().ToLowerInvariant();
        }

        private static string CrearSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var buffer = new byte[16];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

    }
}