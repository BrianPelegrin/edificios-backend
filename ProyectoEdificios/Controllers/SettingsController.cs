using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Data.Contexts;
using ProyectoEdificios.Models.DTO.Settings;
using ProyectoEdificios.Models.Entities.Settings;

namespace ProyectoEdificios.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/unit-colors")]
public sealed partial class SettingsController : ControllerBase
{
    private readonly ProyectoEdificiosDbContext _context;

    public SettingsController(ProyectoEdificiosDbContext context) => _context = context;

    [HttpGet]
    [ProducesResponseType(typeof(List<UnitColorSettingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UnitColorSettingDto>>> Get(CancellationToken cancellationToken)
    {
        var colors = await _context.UnitColorSettings.AsNoTracking()
            .OrderBy(x => x.Estado)
            .Select(x => new UnitColorSettingDto { Id = x.Id, Estado = x.Estado, ColorCss = x.ColorCss })
            .ToListAsync(cancellationToken);
        return Ok(colors);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(SaveUnitColorSettingRequest request, CancellationToken cancellationToken)
    {
        var error = Validate(request, out var estado, out var estadoKey, out var colorCss);
        if (error is not null) return BadRequest(new { message = error });
        if (await _context.UnitColorSettings.AnyAsync(x => x.EstadoKey == estadoKey, cancellationToken))
            return Conflict(new { message = "Ya existe un color para ese estado." });

        var entity = new UnitColorSetting { Estado = estado, EstadoKey = estadoKey, ColorCss = colorCss };
        _context.UnitColorSettings.Add(entity);
        try { await _context.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        { return Conflict(new { message = "Ya existe un color para ese estado." }); }

        var dto = ToDto(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, SaveUnitColorSettingRequest request, CancellationToken cancellationToken)
    {
        var error = Validate(request, out var estado, out var estadoKey, out var colorCss);
        if (error is not null) return BadRequest(new { message = error });

        var entity = await _context.UnitColorSettings.FindAsync([id], cancellationToken);
        if (entity is null) return NotFound();
        if (await _context.UnitColorSettings.AnyAsync(x => x.Id != id && x.EstadoKey == estadoKey, cancellationToken))
            return Conflict(new { message = "Ya existe un color para ese estado." });

        entity.Estado = estado;
        entity.EstadoKey = estadoKey;
        entity.ColorCss = colorCss;
        try { await _context.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        { return Conflict(new { message = "Ya existe un color para ese estado." }); }
        return Ok(ToDto(entity));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _context.UnitColorSettings.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
        return deleted == 0 ? NotFound() : NoContent();
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception) =>
        exception.InnerException is SqlException { Number: 2601 or 2627 };

    private static string? Validate(SaveUnitColorSettingRequest request, out string estado, out string key, out string color)
    {
        estado = request.Estado?.Trim() ?? string.Empty;
        color = request.ColorCss?.Trim() ?? string.Empty;
        key = NormalizeEstadoKey(estado);
        if (estado.Length == 0) return "estado es obligatorio.";
        if (estado.Length > 100) return "estado no puede exceder 100 caracteres.";
        if (!HexColorRegex().IsMatch(color)) return "colorCss debe tener el formato #RRGGBB.";
        return null;
    }

    private static string NormalizeEstadoKey(string value)
    {
        var decomposed = value.Trim().Normalize(NormalizationForm.FormD);
        var chars = decomposed.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC).ToLowerInvariant();
    }

    private static UnitColorSettingDto ToDto(UnitColorSetting x) =>
        new() { Id = x.Id, Estado = x.Estado, ColorCss = x.ColorCss };

    [GeneratedRegex("^#[0-9a-fA-F]{6}$")]
    private static partial Regex HexColorRegex();
}
