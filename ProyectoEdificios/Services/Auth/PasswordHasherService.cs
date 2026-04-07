using Microsoft.AspNetCore.Identity;
using ProyectoEdificios.Models.Entities.Users;

namespace ProyectoEdificios.Services.Auth
{
    public sealed class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public (string Hash, string Salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(password));

            var user = new User
            {
                Email = string.Empty,
                Clave = string.Empty
            };

            var hash = _passwordHasher.HashPassword(user, password);

            return (hash, string.Empty);
        }

        public bool VerifyPassword(string password, string hash, string? salt)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            var user = new User
            {
                Email = string.Empty,
                Clave = hash
            };

            var result = _passwordHasher.VerifyHashedPassword(user, hash, password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}