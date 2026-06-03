using ProyectoEdificios.Models.DTO;

namespace ProyectoEdificios.Services.Projects
{
    public interface IProjectApartmentsService
    {
        Task<ProjectApartmentsResponseDto?> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
        Task<ProjectApartmentsStatsDto?> GetStatsByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
        Task<List<string>> GetSheetListAsync(CancellationToken cancellationToken = default);
    }
}
