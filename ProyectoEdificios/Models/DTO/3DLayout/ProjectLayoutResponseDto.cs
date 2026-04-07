namespace ProyectoEdificios.Models.DTO
{
    public sealed class ProjectLayoutResponseDto
    {
        public ProjectDto Project { get; set; } = new();
        public LayoutDto Layout { get; set; } = new();
    }
}
