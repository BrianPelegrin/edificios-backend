namespace ProyectoEdificios.Models.DTO.Projects
{
    public sealed class CreateProjectDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string ImagenPlano { get; set; } = string.Empty;
    }
}
