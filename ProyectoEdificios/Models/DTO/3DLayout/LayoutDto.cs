namespace ProyectoEdificios.Models.DTO
{
    public class LayoutDto
    {
        public int GridSize { get; set; }
        public List<LayoutBuildingDto> Buildings { get; set; } = [];
    }
}
