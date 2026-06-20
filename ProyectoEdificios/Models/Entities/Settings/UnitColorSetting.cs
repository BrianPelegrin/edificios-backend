namespace ProyectoEdificios.Models.Entities.Settings;

public sealed class UnitColorSetting
{
    public int Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string EstadoKey { get; set; } = string.Empty;
    public string ColorCss { get; set; } = string.Empty;
}
