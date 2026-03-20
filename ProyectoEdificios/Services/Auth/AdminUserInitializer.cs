using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProyectoEdificios.Models.Options;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Models.Entities.Users;
using ProyectoEdificios.Models.Entities.Users;
using ProyectoEdificios.Models.Options;

namespace ProyectoEdificios.Services.Auth
{
    public sealed class AdminUserInitializer
    {
        private readonly ProyectoEdificiosDbContext _context;
        private readonly AdminUserOptions _options;
        private readonly IPasswordHasherService _passwordHasherService;

        public AdminUserInitializer(
            ProyectoEdificiosDbContext context,
            IOptions<AdminUserOptions> options,
            IPasswordHasherService passwordHasherService)
        {
            _context = context;
            _options = options.Value;
            _passwordHasherService = passwordHasherService;
        }

        public async Task EnsureAdminUserExistsAsync(CancellationToken cancellationToken = default)
        {
            var adminExists = await _context.Set<User>()
                .AnyAsync(x => x.Role == "admin", cancellationToken);

            if (adminExists)
                return;

            if (string.IsNullOrWhiteSpace(_options.Email))
                throw new InvalidOperationException("El email del usuario administrador no está configurado.");

            if (string.IsNullOrWhiteSpace(_options.Password))
                throw new InvalidOperationException("La contraseña del usuario administrador no está configurada.");

            var passwordResult = _passwordHasherService.HashPassword(_options.Password);

            var adminUser = new User
            {
                Codigo = string.IsNullOrWhiteSpace(_options.Codigo) ? "ADMIN001" : _options.Codigo.Trim(),
                Nombre = string.IsNullOrWhiteSpace(_options.Nombre) ? "Administrador" : _options.Nombre.Trim(),
                Email = _options.Email.Trim().ToLowerInvariant(),
                Clave = passwordResult.Hash,
                Salt = passwordResult.Salt,
                Role = "admin"
            };

            _context.Set<User>().Add(adminUser);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}