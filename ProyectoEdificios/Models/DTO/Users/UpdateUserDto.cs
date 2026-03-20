namespace ProyectoEdificios.Models.DTO.Users
{
    public sealed class UpdateUserDto
    {
        public string? Codigo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "viewer";
        public bool UpdatePassword { get; set; }
        public string? Password { get; set; }
    }
}