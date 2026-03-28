using ProyectoEdificios.Models.DTO;

namespace ProyectoEdificios.Services.Projects
{
    public interface IProjectApartmentsService
    {
        Task<ProjectApartmentsResponseDto?> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
        List<string> GetSheetList();
    }
}
