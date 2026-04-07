
using ProyectoEdificios.Models.DTO.Auth;
using ProyectoEdificios.Models.Entities.Users;

namespace ProyectoEdificios.Mappings
{
    public static class AuthMappings
    {
        public static AuthUserDto ToAuthUserDto(this User user)
        {
            return new AuthUserDto
            {
                Id = $"usr_{user.Id}",
                Name = user.Nombre ?? string.Empty,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}