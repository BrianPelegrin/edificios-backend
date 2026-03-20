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
        public int Width { get; set; }
        public int Depth { get; set; }
        public int Height { get; set; }

        public ProjectLayout ProjectLayout { get; set; } = default!;
        public ICollection<LayoutUnit> Units { get; set; } = [];
    }
}
