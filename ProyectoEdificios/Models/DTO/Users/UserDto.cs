namespace ProyectoEdificios.Models.DTO.Users
{
    public sealed class UserDto
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}