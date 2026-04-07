namespace ProyectoEdificios.Models.Entities.ProjectLayoutEntities
{
    public class LayoutBuilding
    {
        public required string Id { get; set; } // Ej: bld_1
        public required int ProjectLayoutId { get; set; }
        public required string Name { get; set; }

        public double PositionX { get; set; }
        public double PositionZ { get; set; }
        public double RotationY { get; set; }
        public int LayoutCols { get; set; }
        public int LayoutRows { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }
        public decimal Height { get; set; }

        public ProjectLayout ProjectLayout { get; set; } = default!;
        public ICollection<LayoutUnit> Units { get; set; } = [];
    }
}
