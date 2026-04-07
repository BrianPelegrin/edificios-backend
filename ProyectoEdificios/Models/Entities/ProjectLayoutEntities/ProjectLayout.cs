namespace ProyectoEdificios.Models.Entities.ProjectLayoutEntities
{
    public class ProjectLayout
    {
        public int Id { get; set; }

        public required string ProjectId { get; set; }
        public int GridSize { get; set; }

        public Project Project { get; set; } = default!;
        public ICollection<LayoutBuilding> Buildings { get; set; } = [];
    }
}