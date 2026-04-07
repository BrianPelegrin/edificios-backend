using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Mappings;
using ProyectoEdificios.Models.DTO;
using ProyectoEdificios.Models.DTO.Projects;

namespace ProyectoEdificios.Services.Projects
{
    public sealed class ProjectService : IProjectService
    {
        private readonly ProyectoEdificiosDbContext _context;

        public ProjectService(ProyectoEdificiosDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string? Error, ProjectDto? Project)> CreateAsync(CreateProjectDto request, CancellationToken cancellationToken = default)
        {
            var projectId = request.Id.Trim();

            if (string.IsNullOrWhiteSpace(projectId))
                return (false, "El id del proyecto es obligatorio.", null);

            var exists = await _context.Projects
                .AnyAsync(x => x.Id == projectId, cancellationToken);

            if (exists)
                return (false, "Ya existe un proyecto con ese id.", null);

            var project = request.ToEntity();

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            return (true, null, project.ToDto());
        }

        public async Task<(bool Success, string? Error, ProjectDto? Project)> UpdateAsync(string id, UpdateProjectDto request, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (project is null)
                return (false, "El proyecto no existe.", null);

            request.ApplyToEntity(project);

            await _context.SaveChangesAsync(cancellationToken);

            return (true, null, project.ToDto());
        }

        public async Task<(bool Success, string? Error)> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (project is null)
                return (false, "El proyecto no existe.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);

            return (true, null);
        }
    }
}