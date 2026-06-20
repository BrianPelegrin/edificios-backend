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

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var layoutId = await _context.ProjectLayouts
                    .AsNoTracking()
                    .Where(x => x.ProjectId == projectId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (layoutId == 0)
                {
                    var projectExists = await _context.Projects
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == projectId, cancellationToken);

                    if (!projectExists)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return (false, true, "El proyecto no existe.");
                    }

                    var layout = new ProjectLayout
                    {
                        ProjectId = projectId,
                        GridSize = request.GridSize,
                        BlueprintX = request.BlueprintTransform?.X,
                        BlueprintZ = request.BlueprintTransform?.Z,
                        BlueprintWidth = request.BlueprintTransform?.Width,
                        BlueprintDepth = request.BlueprintTransform?.Depth,
                        BlueprintRotationY = request.BlueprintTransform?.RotationY,
                        BlueprintOpacity = request.BlueprintTransform?.Opacity
                    };

                    _context.ProjectLayouts.Add(layout);
                    await _context.SaveChangesAsync(cancellationToken);
                    layoutId = layout.Id;
                }
                else
                {
                    await _context.ProjectLayouts
                        .Where(x => x.Id == layoutId)
                        .ExecuteUpdateAsync(updates => updates
                            .SetProperty(x => x.GridSize, request.GridSize)
                            .SetProperty(x => x.BlueprintX, request.BlueprintTransform == null ? null : request.BlueprintTransform.X)
                            .SetProperty(x => x.BlueprintZ, request.BlueprintTransform == null ? null : request.BlueprintTransform.Z)
                            .SetProperty(x => x.BlueprintWidth, request.BlueprintTransform == null ? null : request.BlueprintTransform.Width)
                            .SetProperty(x => x.BlueprintDepth, request.BlueprintTransform == null ? null : request.BlueprintTransform.Depth)
                            .SetProperty(x => x.BlueprintRotationY, request.BlueprintTransform == null ? null : request.BlueprintTransform.RotationY)
                            .SetProperty(x => x.BlueprintOpacity, request.BlueprintTransform == null ? null : request.BlueprintTransform.Opacity),
                            cancellationToken);

                    await _context.LayoutUnits
                        .Where(x => x.Building.ProjectLayoutId == layoutId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await _context.LayoutBuildings
                        .Where(x => x.ProjectLayoutId == layoutId)
                        .ExecuteDeleteAsync(cancellationToken);
                }

                var newBuildings = request.Buildings
                    .Select(x => MapBuilding(layoutId, x))
                    .ToList();

                _context.LayoutBuildings.AddRange(newBuildings);

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

            if (request.BlueprintTransform is not null)
            {
                var transform = request.BlueprintTransform;
                if (!double.IsFinite(transform.X) || !double.IsFinite(transform.Z) ||
                    !double.IsFinite(transform.Width) || !double.IsFinite(transform.Depth) ||
                    !double.IsFinite(transform.RotationY) || !double.IsFinite(transform.Opacity))
                    return "Los valores de blueprintTransform deben ser números finitos.";

                if (transform.Width <= 0 || transform.Depth <= 0)
                    return "width y depth de blueprintTransform deben ser mayores que cero.";

                if (transform.Opacity is < 0.15 or > 1)
                    return "opacity de blueprintTransform debe estar entre 0.15 y 1.";
            }

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
                ExternalUnitCode = dto.DetailedUnitCode?.Trim(),
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
