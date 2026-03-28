using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Models.DTO.Projects;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;
using ProyectoEdificios.Models.Enums;


namespace ProyectoEdificios.Services.Projects
{
    public sealed class ProjectLayoutService : IProjectLayoutService
    {
        private readonly ProyectoEdificiosDbContext _context;

        public ProjectLayoutService(ProyectoEdificiosDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, bool NotFound, string? Error)> UpsertAsync(
            string projectId,
            UpsertProject3DLayoutDto request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                return (false, false, "El id del proyecto es obligatorio.");

            if (request is null)
                return (false, false, "El cuerpo de la solicitud es obligatorio.");

            var validationError = ValidateRequest(request);
            if (validationError is not null)
                return (false, false, validationError);

            var project = await _context.Projects
                .Include(x => x.Layout)
                    .ThenInclude(x => x.Buildings)
                        .ThenInclude(x => x.Units)
                .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);

            if (project is null)
                return (false, true, "El proyecto no existe.");

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                ProjectLayout layout;

                if (project.Layout is null)
                {
                    layout = new ProjectLayout
                    {
                        ProjectId = projectId,
                        GridSize = request.GridSize
                    };

                    project.Layout = layout;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    layout = project.Layout;
                    layout.GridSize = request.GridSize;

                    var existingBuildings = layout.Buildings.ToList();
                    var existingUnits = existingBuildings
                        .SelectMany(x => x.Units)
                        .ToList();

                    if (existingUnits.Count > 0)
                        _context.Set<LayoutUnit>().RemoveRange(existingUnits);

                    if (existingBuildings.Count > 0)
                        _context.Set<LayoutBuilding>().RemoveRange(existingBuildings);

                    await _context.SaveChangesAsync(cancellationToken);

                    layout.Buildings.Clear();
                }

                var newBuildings = request.Buildings
                    .Select(x => MapBuilding(layout.Id, x))
                    .ToList();

                foreach (var building in newBuildings)
                {
                    layout.Buildings.Add(building);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return (true, false, null);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private static string? ValidateRequest(UpsertProject3DLayoutDto request)
        {
            if (request.GridSize <= 0)
                return "gridSize debe ser mayor que cero.";

            if (request.Buildings is null)
                return "La colección buildings es obligatoria.";

            var buildingIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var unitIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < request.Buildings.Count; i++)
            {
                var building = request.Buildings[i];

                if (string.IsNullOrWhiteSpace(building.Id))
                    return $"El id del edificio en la posición {i} es obligatorio.";

                var buildingId = building.Id.Trim();

                if (!buildingIds.Add(buildingId))
                    return $"El id del edificio '{building.Id}' está duplicado.";

                if (string.IsNullOrWhiteSpace(building.Name))
                    return $"El nombre del edificio '{building.Id}' es obligatorio.";

                if (building.Position is null)
                    return $"La posición del edificio '{building.Id}' es obligatoria.";

                if (building.Dimensions is null)
                    return $"Las dimensiones del edificio '{building.Id}' son obligatorias.";

                if (building.Dimensions.Width <= 0 ||
                    building.Dimensions.Depth <= 0 ||
                    building.Dimensions.Height <= 0)
                {
                    return $"Las dimensiones del edificio '{building.Id}' deben ser mayores que cero.";
                }

                if (building.Units is null)
                    return $"La colección units del edificio '{building.Id}' es obligatoria.";

                for (int j = 0; j < building.Units.Count; j++)
                {
                    var unit = building.Units[j];

                    if (string.IsNullOrWhiteSpace(unit.Id))
                        return $"El id de la unidad en el edificio '{building.Id}' es obligatorio.";

                    var unitId = unit.Id.Trim();

                    if (!unitIds.Add(unitId))
                        return $"El id de la unidad '{unit.Id}' está duplicado.";

                    if (string.IsNullOrWhiteSpace(unit.Name))
                        return $"El nombre de la unidad '{unit.Id}' es obligatorio.";

                    if (string.IsNullOrWhiteSpace(unit.Status))
                        return $"El status de la unidad '{unit.Id}' es obligatorio.";

                    if (!TryParseUnitStatus(unit.Status, out _))
                        return $"El status '{unit.Status}' de la unidad '{unit.Id}' no es válido.";
                }
            }

            return null;
        }

        private static LayoutBuilding MapBuilding(int projectLayoutId, UpsertLayoutBuildingDto dto)
        {
            var buildingId = dto.Id.Trim();

            return new LayoutBuilding
            {
                Id = buildingId,
                ProjectLayoutId = projectLayoutId,
                Name = dto.Name.Trim(),
                PositionX = dto.Position.X,
                PositionZ = dto.Position.Z,
                RotationY = dto.RotationY,
                LayoutCols = dto.LayoutCols,
                LayoutRows = dto.LayoutRows,
                Width = dto.Dimensions.Width,
                Depth = dto.Dimensions.Depth,
                Height = dto.Dimensions.Height,
                Units = dto.Units
                    .Select(x => MapUnit(buildingId, x))
                    .ToList()
            };
        }

        private static LayoutUnit MapUnit(string layoutBuildingId, UpsertLayoutUnitDto dto)
        {
            TryParseUnitStatus(dto.Status, out var unitStatus);

            return new LayoutUnit
            {
                Id = dto.Id.Trim(),
                LayoutBuildingId = layoutBuildingId,
                Name = dto.Name.Trim(),
                ExternalUnitCode = dto.DetailedUnitCode.Trim(),
                Status = unitStatus,
                Paid = dto.Paid,
                Floor = dto.Floor,
                Slot = dto.Slot
            };
        }

        private static bool TryParseUnitStatus(string rawStatus, out UnitStatus status)
        {
            var normalized = rawStatus.Trim();

            if (Enum.TryParse<UnitStatus>(normalized, true, out status))
                return true;

            status = default;
            return false;
        }
    }
}