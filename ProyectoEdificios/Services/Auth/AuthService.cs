using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Mappings;
using ProyectoEdificios.Models.DTO.Auth;
using ProyectoEdificios.Models.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoEdificios.Services.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly ProyectoEdificiosDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasherService _passwordHasher;


        public AuthService(ProyectoEdificiosDbContext context, IConfiguration configuration, IPasswordHasherService passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim();

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (existingUser is null)
                return null;

            var passwordValid = _passwordHasher.VerifyPassword(request.Password, existingUser.Clave, existingUser.Salt);

            if (!passwordValid)
                return null;

            var accessToken = GenerateAccessToken(existingUser);
            var refreshToken = GenerateRefreshToken();

            existingUser.RefreshTokenHash = HashToken(refreshToken);
            existingUser.RefreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponseDto
            {
                User = existingUser.ToAuthUserDto(),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthUserDto?> GetCurrentUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
                return null;

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
                return null;

            return user.ToAuthUserDto();
        }

        private string GenerateAccessToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
            var jwtIssuer = jwtSection["Issuer"];
            var jwtAudience = jwtSection["Audience"];

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Nombre ?? string.Empty),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        private static string CrearSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var buff = new byte[16];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

    }
}