using ProyectoEdificios.Models.DTO.Auth;
using ProyectoEdificios.Models.DTO.Users;
using ProyectoEdificios.Models.Entities.Users;
using System.Linq.Expressions;

namespace ProyectoEdificios.Mappings
{
    public static class UserMappings
    {
        public static readonly Expression<Func<User, UserDto>> ToDtoExpression = user => new UserDto
        {
            Id = user.Id,
            Codigo = user.Codigo,
            Name = user.Nombre ?? string.Empty,
            Email = user.Email,
            Role = user.Role
        };

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Codigo = user.Codigo,
                Name = user.Nombre ?? string.Empty,
                Email = user.Email,
                Role = user.Role
            };
        }

    }
}