namespace ProyectoEdificios.Models.DTO.Projects
{
    public sealed class UpsertProject3DLayoutDto
    {
        public int GridSize { get; set; }
        public List<UpsertLayoutBuildingDto> Buildings { get; set; } = [];
    }

    public sealed class UpsertLayoutBuildingDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double RotationY { get; set; }
        public UpsertLayoutPositionDto Position { get; set; } = new();
        public UpsertLayoutDimensionsDto Dimensions { get; set; } = new();
        public List<UpsertLayoutUnitDto> Units { get; set; } = [];
    }

    public sealed class UpsertLayoutPositionDto
    {
        public double X { get; set; }
        public double Z { get; set; }
    }

    public sealed class UpsertLayoutDimensionsDto
    {
        public int Width { get; set; }
        public int Depth { get; set; }
        public int Height { get; set; }
    }

    public sealed class UpsertLayoutUnitDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool Paid { get; set; }
        public string? DetailedUnitCode { get; set; } = string.Empty;
    }

    public sealed class SuccessResponseDto
    {
        public bool Success { get; set; }
    }
}