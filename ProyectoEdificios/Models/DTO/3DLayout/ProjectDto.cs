namespace ProyectoEdificios.Models.DTO
{
    public class ProjectDto
    {
        public string Id { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public string Direccion { get; set; } = default!;
        public string Provincia { get; set; } = default!;
        public string Municipio { get; set; } = default!;
        public string ImagenPlano { get; set; } = default!;
    }
}
