namespace ProyectoEdificios.Models.DTO.Users
{
    public sealed class CreateUserDto
    {
        public string? Codigo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "viewer";
    }
}