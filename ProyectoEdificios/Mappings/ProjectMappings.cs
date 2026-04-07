using ProyectoEdificios.Models.DTO;
using ProyectoEdificios.Models.DTO.Projects;
using System.Linq.Expressions;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Mappings
{
    public static class ProjectMappings
    {
        public static readonly Expression<Func<Project, ProjectDto>> ToDtoExpression = project => new ProjectDto
        {
            Id = project.Id,
            Nombre = project.Name,
            Direccion = project.Address,
            Provincia = project.Province,
            Municipio = project.Municipality,
            ImagenPlano = project.PlanImageUrl
        };

        public static ProjectDto ToDto(this Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Nombre = project.Name,
                Direccion = project.Address,
                Provincia = project.Province,
                Municipio = project.Municipality,
                ImagenPlano = project.PlanImageUrl
            };
        }

        public static Project ToEntity(this CreateProjectDto dto)
        {
            return new Project
            {
                Id = dto.Id.Trim(),
                Name = dto.Nombre.Trim(),
                Address = dto.Direccion.Trim(),
                Province = dto.Provincia.Trim(),
                Municipality = dto.Municipio.Trim(),
                PlanImageUrl = dto.ImagenPlano.Trim()
            };
        }

        public static void ApplyToEntity(this UpdateProjectDto dto, Project project)
        {
            project.Name = dto.Nombre.Trim();
            project.Address = dto.Direccion.Trim();
            project.Province = dto.Provincia.Trim();
            project.Municipality = dto.Municipio.Trim();
            project.PlanImageUrl = dto.ImagenPlano.Trim();
        }
    }
}
