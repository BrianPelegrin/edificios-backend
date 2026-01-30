using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoEdificios.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace ProyectoEdificios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ProyectoEdificiosDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(ProyectoEdificiosDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ----------------- REGISTER -----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest(new { message = "El correo ya está registrado." });

            var salt = CrearSalt();
            var hashedPassword = GenerarHash(user.Clave, salt);

            var newUser = new User
            {
                Codigo = user.Codigo,
                Nombre = user.Nombre,
                Email = user.Email,
                Clave = hashedPassword,
                Salt = salt
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente." });
        }

        // ----------------- LOGIN -----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null)
                return BadRequest(new { message = "Usuario o contraseña inválidos." });

            bool passwordValid = ValidarPassword(user.Clave, existingUser.Clave, existingUser.Salt);
            if (!passwordValid)
                return BadRequest(new { message = "Usuario o contraseña inválidos." });

            var jwtSection = _configuration.GetSection("Jwt");
            var jwtKey = jwtSection["key"];
            var jwtIssuer = jwtSection["Issuer"];
            var jwtAudience = jwtSection["Audience"];

                var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSection["Subject"] ?? "UserToken"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", existingUser.Email),
                new Claim("Nombre", existingUser.Nombre)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new
            {
                user = new { existingUser.Id, existingUser.Nombre, existingUser.Email },
                token = tokenString
            });
        }

        // ----------------- GET ALL USERS -----------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    Codigo = u.Codigo,
                    Nombre = u.Nombre,
                    Email = u.Email
                }).ToListAsync();
        }

        // ----------------- UTILIDADES -----------------
        private static string CrearSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var buff = new byte[16];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        private static string GenerarHash(string input, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(input + salt);
            var hash = SHA256.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool ValidarPassword(string password, string hashGuardado, string salt)
        {
            string hashNuevo = GenerarHash(password, salt);
            return hashNuevo == hashGuardado;
        }
    }
}
