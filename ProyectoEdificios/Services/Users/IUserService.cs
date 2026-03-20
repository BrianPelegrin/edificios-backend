using ProyectoEdificios.Models.DTO.Users;

namespace ProyectoEdificios.Services.Users
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<(bool Success, string? Error, UserDto? User)> CreateAsync(CreateUserDto request, CancellationToken cancellationToken = default);
        Task<(bool Success, string? Error, UserDto? User)> UpdateAsync(int id, UpdateUserDto request, CancellationToken cancellationToken = default);
        Task<(bool Success, string? Error)> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}