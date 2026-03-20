using ProyectoEdificios.Models.DTO.Auth;

public sealed class LoginResponseDto
{
    public AuthUserDto User { get; set; } = new();
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}