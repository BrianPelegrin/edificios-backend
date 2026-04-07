namespace ProyectoEdificios.Services.Auth
{
    public interface IPasswordHasherService
    {
        (string Hash, string Salt) HashPassword(string password);
        bool VerifyPassword(string password, string hash, string? salt);
    }
}
