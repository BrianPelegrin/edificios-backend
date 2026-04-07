using ProyectoEdificios.Models.DTO;
using ProyectoEdificios.Models.DTO.Projects;

namespace ProyectoEdificios.Services.Projects
{
    public interface IProjectService
    {
        Task<(bool Success, string? Error, ProjectDto? Project)> CreateAsync(CreateProjectDto request, CancellationToken cancellationToken = default);
        Task<(bool Success, string? Error, ProjectDto? Project)> UpdateAsync(string id, UpdateProjectDto request, CancellationToken cancellationToken = default);
        Task<(bool Success, string? Error)> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
