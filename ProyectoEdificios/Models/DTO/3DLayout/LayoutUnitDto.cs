namespace ProyectoEdificios.Models.DTO
{
    public class LayoutUnitDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Floor { get; set; }              // 1..N
        public int Slot { get; set; }               // 0..(LayoutCols*LayoutRows - 1)
        public string? ExternalUnitCode => this.DetailedUnitCode;
        public string? DetailedUnitCode { get; set; } = default!;
        public string Status { get; set; } = default!;
        public bool Paid { get; set; }

    }

    
}
