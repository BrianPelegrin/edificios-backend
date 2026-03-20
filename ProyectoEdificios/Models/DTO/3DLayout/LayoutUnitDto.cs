namespace ProyectoEdificios.Models.DTO
{
    public class LayoutUnitDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? ExternalUnitCode => this.DetailedUnitCode;
        public string? DetailedUnitCode { get; set; } = default!;
        public string Status { get; set; } = default!;
        public bool Paid { get; set; }
    }

    
}
