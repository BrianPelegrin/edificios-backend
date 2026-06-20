namespace ProyectoEdificios.Models.Entities.ProjectLayoutEntities
{
    public class ProjectLayout
    {
        public int Id { get; set; }

        public required string ProjectId { get; set; }
        public int GridSize { get; set; }
        public double? BlueprintX { get; set; }
        public double? BlueprintZ { get; set; }
        public double? BlueprintWidth { get; set; }
        public double? BlueprintDepth { get; set; }
        public double? BlueprintRotationY { get; set; }
        public double? BlueprintOpacity { get; set; }

        public Project Project { get; set; } = default!;
        public ICollection<LayoutBuilding> Buildings { get; set; } = [];
    }
}
