namespace ProyectoEdificios.Models.DTO.Projects;

public sealed class ProjectListItemDto
{
    public string Id { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public string Direccion { get; set; } = default!;
    public string Provincia { get; set; } = default!;
    public string Municipio { get; set; } = default!;
}
