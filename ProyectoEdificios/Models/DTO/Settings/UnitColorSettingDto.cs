namespace ProyectoEdificios.Models.DTO.Settings;

public sealed class UnitColorSettingDto
{
    public int Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string ColorCss { get; set; } = string.Empty;
}

public sealed class SaveUnitColorSettingRequest
{
    public int? Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Color { get; set; }
    public string ColorCss { get; set; } = string.Empty;
}
