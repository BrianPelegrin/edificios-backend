namespace ProyectoEdificios.Models.Options
{
    public sealed class AdminUserOptions
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
