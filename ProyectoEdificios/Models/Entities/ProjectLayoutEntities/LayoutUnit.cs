using ProyectoEdificios.Models.Enums;

namespace ProyectoEdificios.Models.Entities.ProjectLayoutEntities
{
    public class LayoutUnit
    {
        public required string Id { get; set; } // Ej: unt_1
        public required string LayoutBuildingId { get; set; }
        public required string Name { get; set; }
        public string? ExternalUnitCode { get; set; }

        public UnitStatus Status { get; set; }
        public bool Paid { get; set; }

        public LayoutBuilding Building { get; set; } = default!;
    }
}
