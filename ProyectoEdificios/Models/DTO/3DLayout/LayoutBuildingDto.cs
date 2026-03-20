namespace ProyectoEdificios.Models.DTO
{
    public class LayoutBuildingDto
    {

        public string Id { get; set; } = default!;
        public string ProjectId { get; set; } = default!;
        public double RotationY { get; set; }
        public string Name { get; set; } = default!;
        public PositionDto Position { get; set; } = new();
        public DimensionsDto Dimensions { get; set; } = new();
        public List<LayoutUnitDto> Units { get; set; } = [];
    }

    public class PositionDto
    {
        public double X { get; set; }
        public double Z { get; set; }
    }

    public class DimensionsDto
    {
        public int Width { get; set; }
        public int Depth { get; set; }
        public int Height { get; set; }
    }
}
