using ProyectoEdificios.Models.DTO.Projects;

namespace ProyectoEdificios.Services.Projects
{
    public interface IProjectLayoutService
    {
        Task<(bool Success, bool NotFound, string? Error)> UpsertAsync(
            string projectId,
            UpsertProject3DLayoutDto request,
            CancellationToken cancellationToken = default);
    }
}