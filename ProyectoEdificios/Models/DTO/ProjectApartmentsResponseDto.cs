namespace ProyectoEdificios.Models.DTO
{
    public sealed class ProjectApartmentsResponseDto
    {
        public string ProjectId { get; set; } = default!;
        public List<ApartmentDto> Apartments { get; set; } = [];
    }
}
