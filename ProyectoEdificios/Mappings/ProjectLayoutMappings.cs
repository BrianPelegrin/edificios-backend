using ProyectoEdificios.Models.DTO;
using ProyectoEdificios.Models.Enums;
using System.Linq.Expressions;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Mappings
{
    public static class ProjectLayoutMappings
    {
        public static readonly Expression<Func<Project, Project3DLayoutDto>> To3DLayoutDtoExpression = project =>
            new Project3DLayoutDto
            {
                ProjectId = project.Id,
                GridSize = project.Layout!.GridSize,
                BlueprintTransform = project.Layout.BlueprintWidth == null
                    ? null
                    : new BlueprintTransformDto
                    {
                        X = project.Layout.BlueprintX!.Value,
                        Z = project.Layout.BlueprintZ!.Value,
                        Width = project.Layout.BlueprintWidth.Value,
                        Depth = project.Layout.BlueprintDepth!.Value,
                        RotationY = project.Layout.BlueprintRotationY!.Value,
                        Opacity = project.Layout.BlueprintOpacity!.Value
                    },
                Buildings = project.Layout.Buildings
                    .OrderBy(building => building.Name)
                    .Select(building => new LayoutBuildingDto
                    {
                        Id = building.Id,
                        ProjectId = project.Id,
                        Name = building.Name,
                        RotationY = building.RotationY,
                        LayoutRows = building.LayoutRows,
                        LayoutCols = building.LayoutCols,
                        Position = new PositionDto
                        {
                            X = building.PositionX,
                            Z = building.PositionZ
                        },
                        Dimensions = new DimensionsDto
                        {
                            Width = building.Width,
                            Depth = building.Depth,
                            Height = building.Height
                        },
                        Units = building.Units
                            .OrderBy(unit => unit.Name)
                            .Select(unit => new LayoutUnitDto
                            {
                                Id = unit.Id,
                                Name = unit.Name,
                                Status = unit.Status == UnitStatus.Available ? "available"
                                    : unit.Status == UnitStatus.Reserved ? "reserved"
                                    : unit.Status == UnitStatus.Sold ? "sold"
                                    : "delivered",
                                Paid = unit.Paid,
                                DetailedUnitCode = unit.ExternalUnitCode,
                                Slot = unit.Slot,
                                Floor = unit.Floor,
                            })
                            .ToList()
                    })
                    .ToList()
            };

        public static Project3DLayoutDto To3DLayoutDto(this Project project)
        {
            ArgumentNullException.ThrowIfNull(project);

            if (project.Layout is null)
                throw new InvalidOperationException($"The project '{project.Id}' does not have an associated layout.");

            return new Project3DLayoutDto
            {
                ProjectId = project.Id,
                GridSize = project.Layout.GridSize,
                BlueprintTransform = MapBlueprintTransform(project.Layout),
                Buildings = project.Layout.Buildings
                    .OrderBy(building => building.Name)
                    .Select(building => building.ToDto(project.Id))
                    .ToList()
            };
        }

        public static LayoutBuildingDto ToDto(this LayoutBuilding building, string projectId)
        {
            ArgumentNullException.ThrowIfNull(building);

            return new LayoutBuildingDto
            {
                Id = building.Id,
                ProjectId = projectId,
                Name = building.Name,
                RotationY = building.RotationY,
                LayoutRows = building.LayoutRows,
                LayoutCols = building.LayoutCols,
                Position = new PositionDto
                {
                    X = building.PositionX,
                    Z = building.PositionZ
                },
                Dimensions = new DimensionsDto
                {
                    Width = building.Width,
                    Depth = building.Depth,
                    Height = building.Height
                },
                Units = building.Units
                    .OrderBy(unit => unit.Name)
                    .Select(unit => unit.ToDto())
                    .ToList()
            };
        }

        public static LayoutUnitDto ToDto(this LayoutUnit unit)
        {
            ArgumentNullException.ThrowIfNull(unit);

            return new LayoutUnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Status = MapStatus(unit.Status),
                Paid = unit.Paid,
                DetailedUnitCode = unit.ExternalUnitCode,
                Slot = unit.Slot,
                Floor = unit.Floor,
            };
        }

        private static string MapStatus(UnitStatus status)
        {
            return status switch
            {
                UnitStatus.Available => "available",
                UnitStatus.Reserved => "reserved",
                UnitStatus.Sold => "sold",
                UnitStatus.Delivered => "delivered",
                _ => "available"
            };
        }

        private static BlueprintTransformDto? MapBlueprintTransform(ProjectLayout layout)
        {
            if (layout.BlueprintWidth is null)
                return null;

            return new BlueprintTransformDto
            {
                X = layout.BlueprintX!.Value,
                Z = layout.BlueprintZ!.Value,
                Width = layout.BlueprintWidth.Value,
                Depth = layout.BlueprintDepth!.Value,
                RotationY = layout.BlueprintRotationY!.Value,
                Opacity = layout.BlueprintOpacity!.Value
            };
        }
    }
}
