namespace ProyectoEdificios.Models.Entities.Users
{
    public partial class User
    {
        public int Id { get; set; }

        public string? Codigo { get; set; }

        public string? Nombre { get; set; }

        public string Email { get; set; }

        public string Clave { get; set; }

        public string? Salt { get; set; }

        public string Role { get; set; } = "viewer";

        public string? RefreshTokenHash { get; set; }

        public DateTime? RefreshTokenExpiresAtUtc { get; set; }
    }
}
