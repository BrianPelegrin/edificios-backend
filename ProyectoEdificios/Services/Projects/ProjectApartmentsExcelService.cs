using ClosedXML.Excel;
using ProyectoEdificios.Models.DTO;
using System.Globalization;

namespace ProyectoEdificios.Services.Projects
{
    public sealed class ProjectApartmentsExcelService : IProjectApartmentsService
    {
        private readonly IWebHostEnvironment _environment;

        public ProjectApartmentsExcelService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<ProjectApartmentsResponseDto?> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Data", "control2.xlsx");

            if (!File.Exists(filePath))
                return Task.FromResult<ProjectApartmentsResponseDto?>(null);

            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheets.FirstOrDefault(ws =>
                ws.Name.Equals(projectId, StringComparison.OrdinalIgnoreCase));

            if (worksheet is null)
                return Task.FromResult<ProjectApartmentsResponseDto?>(null);

            var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1).ToList() ?? new List<IXLRangeRow>();

            var apartments = rows
                .Select(row => MapRowToDto(row))
                .ToList();

            var response = new ProjectApartmentsResponseDto
            {
                ProjectId = projectId,
                Apartments = apartments
            };

            return Task.FromResult<ProjectApartmentsResponseDto?>(response);
        }

        private static ApartmentDto MapRowToDto(IXLRangeRow row)
        {
            return new ApartmentDto
            {
                Id = row.RowNumber(),
                CodUnidad = GetString(row.Cell(1)),
                Edificio = GetString(row.Cell(2)),
                Unidad = GetString(row.Cell(3)),
                Metraje = ParseDecimal(row.Cell(4)),
                Estado = GetString(row.Cell(5)),
                Nombre = GetString(row.Cell(6)),
                Telefono = GetString(row.Cell(7)),
                Correo = GetString(row.Cell(8)),
                Cedula = GetString(row.Cell(9)),
                Precio = ParseDecimal(row.Cell(10)),
                Inicial = ParseDecimal(row.Cell(11)),
                FechaCompletaInicial = ParseDate(row.Cell(12)),
                InicialDolar = ParseDecimal(row.Cell(13)),
                Pagado = ParseDecimal(row.Cell(14)),
                Adeudado = ParseDecimal(row.Cell(15)),
                IniciadoVaciados = ParseBool(row.Cell(16)) ?? false,
                FechaInicioVaciados = ParseDate(row.Cell(17)),
                EnInspeccion = ParseBool(row.Cell(18)) ?? false,
                FechaEntregaInspeccion = ParseDate(row.Cell(19)),
                Legal = ParseBool(row.Cell(20)) ?? false,
                ResponsableLegal = GetString(row.Cell(21)),
                FechaLegal = ParseDate(row.Cell(22)),
                Gobierno = ParseBool(row.Cell(23)) ?? false,
                ResponsableGobierno = GetString(row.Cell(24)),
                FechaGobierno = ParseDate(row.Cell(25)),
                Micelaneos = ParseBool(row.Cell(26)) ?? false,
                ResponsableMicelaneos = GetString(row.Cell(27)),
                FechaMicelaneos = ParseDate(row.Cell(28)),
                Inspeccion1 = ParseBool(row.Cell(29)) ?? false,
                FechaInspeccion1 = ParseDate(row.Cell(30)),
                Inspeccion2 = ParseBool(row.Cell(31)) ?? false,
                FechaInspeccion2 = ParseDate(row.Cell(32)),
                FormaPago = GetString(row.Cell(33)),
                FechaFormaPago = ParseDate(row.Cell(34)),
                Banco = GetString(row.Cell(35)),
                Saldo = ParseBool(row.Cell(36)) ?? false,
                Entregada = ParseBool(row.Cell(37)) ?? false,
                Titulo = ParseBool(row.Cell(38)) ?? false,
                DescargadaDGII = ParseBool(row.Cell(39)) ?? false
            };
        }

        private static string GetString(IXLCell cell)
        {
            return cell.GetString().Trim();
        }

        private static bool? ParseBool(IXLCell cell)
        {
            var value = cell.GetString().Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (value is "SI" or "YES" or "TRUE" or "1")
                return true;

            if (value is "NO" or "FALSE" or "0")
                return false;

            return null;
        }

        private static decimal? ParseDecimal(IXLCell cell)
        {
            var raw = cell.GetString().Trim();

            if (string.IsNullOrWhiteSpace(raw))
                return null;

            raw = raw.Replace("RD$", "", StringComparison.OrdinalIgnoreCase)
                     .Replace("$", "")
                     .Replace("%", "")
                     .Replace(",", "")
                     .Trim();

            if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;

            return null;
        }

        private static DateOnly? ParseDate(IXLCell cell)
        {
            if (cell.IsEmpty())
                return null;

            if (cell.DataType == XLDataType.DateTime)
                return DateOnly.FromDateTime(cell.GetDateTime());

            if (cell.DataType == XLDataType.Number)
            {
                try
                {
                    return DateOnly.FromDateTime(DateTime.FromOADate(cell.GetDouble()));
                }
                catch
                {
                    return null;
                }
            }

            var raw = cell.GetString().Trim();

            if (string.IsNullOrWhiteSpace(raw) || raw is "N/A" or "--" or "-")
                return null;

            if (DateTime.TryParse(raw, new CultureInfo("es-DO"), DateTimeStyles.None, out var date))
                return DateOnly.FromDateTime(date);

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return DateOnly.FromDateTime(date);

            return null;
        }
    }
}