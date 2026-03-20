using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Mappings;
using ProyectoEdificios.Models.DTO;
using ProyectoEdificios.Models.DTO.Projects;
using ProyectoEdificios.Services.Projects;

namespace ProyectoEdificios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProyectoEdificiosDbContext _context;
        private readonly IProjectApartmentsService _projectApartmentsService;
        private readonly IProjectService _projectService;
        private readonly IProjectLayoutService _projectLayoutService;

        public ProjectsController(
            ProyectoEdificiosDbContext context,
            IProjectApartmentsService projectApartmentsService,
            IProjectService projectService,
            IProjectLayoutService projectLayoutService)
        {
            _context = context;
            _projectApartmentsService = projectApartmentsService;
            _projectService = projectService;
            _projectLayoutService = projectLayoutService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
        {
            var projects = await _context.Projects
                .AsNoTracking()
                .Select(ProjectMappings.ToDtoExpression)
                .ToListAsync(cancellationToken);

            return Ok(projects);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(string id, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(ProjectMappings.ToDtoExpression)
                .FirstOrDefaultAsync(cancellationToken);

            if (project is null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("{id}/layout")]
        [ProducesResponseType(typeof(Project3DLayoutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectLayout(string id, CancellationToken cancellationToken)
        {
            var projectExists = await _context.Projects
                .AsNoTracking()
                .AnyAsync(x => x.Id == id, cancellationToken);

            if (!projectExists)
                return NotFound(new { message = $"Project '{id}' was not found." });

            var response = await _context.Projects
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Where(x => x.Layout != null)
                .Select(ProjectLayoutMappings.To3DLayoutDtoExpression)
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
                return NotFound(new { message = $"Project '{id}' does not have a layout configured." });

            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}/layout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpsertProjectLayout(string id, [FromBody] UpsertProject3DLayoutDto request, CancellationToken cancellationToken)
        {
            var result = await _projectLayoutService.UpsertAsync(id, request, cancellationToken);

            if (!result.Success && result.Error == "El proyecto no existe.")
                return NotFound(new { message = result.Error });

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { success = true });
        }

        [HttpGet("{id}/apartments")]
        [ProducesResponseType(typeof(ProjectApartmentsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectApartments(string id, CancellationToken cancellationToken)
        {
            var response = await _projectApartmentsService.GetByProjectIdAsync(id, cancellationToken);

            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto request, CancellationToken cancellationToken)
        {
            var result = await _projectService.CreateAsync(request, cancellationToken);

            if (!result.Success && result.Error == "Ya existe un proyecto con ese id.")
                return Conflict(new { message = result.Error });

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetProjectById), new { id = result.Project!.Id }, result.Project);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] UpdateProjectDto request, CancellationToken cancellationToken)
        {
            var result = await _projectService.UpdateAsync(id, request, cancellationToken);

            if (!result.Success && result.Error == "El proyecto no existe.")
                return NotFound(new { message = result.Error });

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(result.Project);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject(string id, CancellationToken cancellationToken)
        {
            var result = await _projectService.DeleteAsync(id, cancellationToken);

            if (!result.Success)
                return NotFound(new { message = result.Error });

            return NoContent();
        }
    }
}