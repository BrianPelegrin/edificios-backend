namespace ProyectoEdificios.Models.DTO.Users
{
    public sealed class UpdateMyProfileDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? OldPassword { get; set; }
    }
}
