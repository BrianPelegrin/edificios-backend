namespace ProyectoEdificios.Models.DTO
{
    public sealed class Project3DLayoutDto
    {
        public string ProjectId { get; set; } = default!;
        public int GridSize { get; set; }
        public List<LayoutBuildingDto> Buildings { get; set; } = [];
    }
}
