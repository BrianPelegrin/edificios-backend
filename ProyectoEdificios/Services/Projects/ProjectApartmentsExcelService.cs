using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ProyectoEdificios.Models.DTO;
using System.Globalization;

namespace ProyectoEdificios.Services.Projects
{
    public sealed class ProjectApartmentsExcelService : IProjectApartmentsService
    {
        private readonly IChangeControlWorkbookSource _workbookSource;
        private readonly ILogger<ProjectApartmentsExcelService> _logger;

        public ProjectApartmentsExcelService(
            IChangeControlWorkbookSource workbookSource,
            ILogger<ProjectApartmentsExcelService> logger)
        {
            _workbookSource = workbookSource;
            _logger = logger;
        }

        public async Task<List<string>> GetSheetListAsync(CancellationToken cancellationToken = default)
        {
            using var workbookStream = await _workbookSource.OpenReadAsync(cancellationToken);

            if (workbookStream is null)
                return new List<string>();

            using var document = SpreadsheetDocument.Open(workbookStream, false);
            var workbookPart = document.WorkbookPart;

            if (workbookPart?.Workbook.Sheets is null)
                return new List<string>();

            return workbookPart.Workbook.Sheets.Elements<Sheet>()
                .Select(sheet => sheet.Name?.Value)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Cast<string>()
                .ToList();
        }

        public async Task<ProjectApartmentsResponseDto?> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default)
        {
            var apartments = await GetProjectApartmentsAsync(projectId, cancellationToken);

            if (apartments is null)
                return null;

            var response = new ProjectApartmentsResponseDto
            {
                ProjectId = projectId,
                Apartments = apartments
            };

            return response;
        }

        public async Task<ProjectApartmentsStatsDto?> GetStatsByProjectIdAsync(string projectId, CancellationToken cancellationToken = default)
        {
            var apartments = await GetProjectApartmentsAsync(projectId, cancellationToken);

            if (apartments is null)
                return null;

            return new ProjectApartmentsStatsDto
            {
                ProjectId = projectId,
                Edificios = apartments
                    .Select(apartment => apartment.Edificio?.Trim())
                    .Where(edificio => !string.IsNullOrWhiteSpace(edificio))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count(),
                Vendida = apartments.Count(apartment => IsVendida(apartment.Estado)),
                TotalUnidades = apartments.Count,
                UnidadesEnInspeccion = apartments.Count(apartment => apartment.EnInspeccion),
                DisponiblesObservacion = apartments.Count(apartment => IsDisponibleOrObservacion(apartment.Estado))
            };
        }

        private static ApartmentDto MapRowToDto(
            Row row,
            WorkbookPart workbookPart,
            IReadOnlyDictionary<string, DateOnly> deliveryDatesByUnit)
        {
            var codUnidad = GetString(row, 1, workbookPart);
            var fechaEntrega = ParseDate(GetString(row, 41, workbookPart));

            if (fechaEntrega is null
                && !string.IsNullOrWhiteSpace(codUnidad)
                && deliveryDatesByUnit.TryGetValue(codUnidad, out var deliveryDate))
            {
                fechaEntrega = deliveryDate;
            }

            return new ApartmentDto
            {
                Id = (int)(row.RowIndex?.Value ?? 0),
                CodUnidad = codUnidad,
                Edificio = GetString(row, 2, workbookPart),
                Unidad = GetString(row, 3, workbookPart),
                Metraje = ParseDecimal(GetString(row, 4, workbookPart)),
                Estado = GetString(row, 5, workbookPart),
                Nombre = GetString(row, 6, workbookPart),
                Telefono = GetString(row, 7, workbookPart),
                Correo = GetString(row, 8, workbookPart),
                Cedula = GetString(row, 9, workbookPart),
                Precio = ParseDecimal(GetString(row, 10, workbookPart)),
                Inicial = ParseDecimal(GetString(row, 11, workbookPart)),
                FechaCompletaInicial = ParseDate(GetString(row, 12, workbookPart)),
                InicialDolar = ParseDecimal(GetString(row, 13, workbookPart)),
                Pagado = ParseDecimal(GetString(row, 14, workbookPart)),
                Adeudado = ParseDecimal(GetString(row, 15, workbookPart)),
                IniciadoVaciados = ParseBool(GetString(row, 16, workbookPart)) ?? false,
                FechaInicioVaciados = ParseDate(GetString(row, 17, workbookPart)),
                EnInspeccion = ParseBool(GetString(row, 18, workbookPart)) ?? false,
                FechaEntregaInspeccion = ParseDate(GetString(row, 19, workbookPart)),
                Legal = ParseBool(GetString(row, 20, workbookPart)) ?? false,
                ResponsableLegal = GetString(row, 21, workbookPart),
                FechaLegal = ParseDate(GetString(row, 22, workbookPart)),
                Gobierno = ParseBool(GetString(row, 23, workbookPart)) ?? false,
                ResponsableGobierno = GetString(row, 24, workbookPart),
                FechaGobierno = ParseDate(GetString(row, 25, workbookPart)),
                Micelaneos = ParseBool(GetString(row, 26, workbookPart)) ?? false,
                ResponsableMicelaneos = GetString(row, 27, workbookPart),
                FechaMicelaneos = ParseDate(GetString(row, 28, workbookPart)),
                Inspeccion1 = ParseBool(GetString(row, 29, workbookPart)) ?? false,
                FechaInspeccion1 = ParseDate(GetString(row, 30, workbookPart)),
                Inspeccion2 = ParseBool(GetString(row, 31, workbookPart)) ?? false,
                FechaInspeccion2 = ParseDate(GetString(row, 32, workbookPart)),
                FormaPago = GetString(row, 33, workbookPart),
                FechaFormaPago = ParseDate(GetString(row, 34, workbookPart)),
                Banco = GetString(row, 35, workbookPart),
                Saldo = ParseBool(GetString(row, 36, workbookPart)) ?? false,
                Entregada = ParseBool(GetString(row, 37, workbookPart)) ?? false,
                Titulo = ParseBool(GetString(row, 38, workbookPart)) ?? false,
                DescargadaDGII = ParseBool(GetString(row, 39, workbookPart)) ?? false,
                FechaEntrega = fechaEntrega
            };
        }

        private async Task<List<ApartmentDto>?> GetProjectApartmentsAsync(string projectId, CancellationToken cancellationToken)
        {
            using var workbookStream = await _workbookSource.OpenReadAsync(cancellationToken);

            if (workbookStream is null)
                return null;

            using var document = SpreadsheetDocument.Open(workbookStream, false);
            var workbookPart = document.WorkbookPart;
            var worksheet = workbookPart?.Workbook.Sheets?.Elements<Sheet>().FirstOrDefault(sheet =>
                string.Equals(sheet.Name?.Value, projectId, StringComparison.OrdinalIgnoreCase));

            if (worksheet is null)
            {
                _logger.LogInformation("Worksheet for project {ProjectId} was not found in the change control workbook.", projectId);
                return null;
            }

            var worksheetPart = (WorksheetPart)workbookPart!.GetPartById(worksheet.Id!.Value!);
            var rows = GetApartmentRows(worksheetPart, workbookPart);
            var deliveryDatesByUnit = GetDeliveryDatesByUnit(workbookPart);

            return rows
                .Select(row => MapRowToDto(row, workbookPart, deliveryDatesByUnit))
                .ToList();
        }

        private static Dictionary<string, DateOnly> GetDeliveryDatesByUnit(WorkbookPart workbookPart)
        {
            var worksheet = workbookPart.Workbook.Sheets?.Elements<Sheet>().FirstOrDefault(sheet =>
                string.Equals(sheet.Name?.Value, "Entregas", StringComparison.OrdinalIgnoreCase));

            if (worksheet?.Id?.Value is null)
                return new Dictionary<string, DateOnly>(StringComparer.OrdinalIgnoreCase);

            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(worksheet.Id.Value);
            var rows = worksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>() ?? Enumerable.Empty<Row>();
            var deliveryDatesByUnit = new Dictionary<string, DateOnly>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var codUnidad = GetString(row, 3, workbookPart);

                if (string.IsNullOrWhiteSpace(codUnidad))
                    continue;

                var deliveryDate = ParseDate(GetString(row, 6, workbookPart));

                if (deliveryDate is not null)
                    deliveryDatesByUnit[codUnidad] = deliveryDate.Value;
            }

            return deliveryDatesByUnit;
        }

        private static List<Row> GetApartmentRows(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            var rows = worksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>().ToList() ?? new List<Row>();

            if (rows.Count == 0)
                return rows;

            var headerIndex = rows.FindIndex(row =>
                string.Equals(GetString(row, 1, workbookPart), "unidad", StringComparison.OrdinalIgnoreCase));

            if (headerIndex >= 0)
                return rows.Skip(headerIndex + 1)
                    .Where(row => IsApartmentRow(row, workbookPart))
                    .ToList();

            return rows.Skip(1)
                .Where(row => IsApartmentRow(row, workbookPart))
                .ToList();
        }

        private static bool IsApartmentRow(Row row, WorkbookPart workbookPart)
        {
            var codUnidad = GetString(row, 1, workbookPart);
            var unidad = GetString(row, 3, workbookPart);
            var estado = GetString(row, 5, workbookPart);

            return !string.IsNullOrWhiteSpace(codUnidad)
                || !string.IsNullOrWhiteSpace(unidad)
                || !string.IsNullOrWhiteSpace(estado);
        }

        private static bool IsDisponibleOrObservacion(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return false;

            var normalized = estado.Trim();
            return normalized.Contains("disponible", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("observacion", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("observación", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsVendida(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return false;

            var normalized = estado.Trim();
            return normalized.Contains("vendida", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("vendido", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetString(Row row, int columnIndex, WorkbookPart workbookPart)
        {
            var cell = GetCell(row, columnIndex);
            return GetCellValue(cell, workbookPart).Trim();
        }

        private static bool? ParseBool(string value)
        {
            value = value.Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (value is "SI" or "YES" or "TRUE" or "1")
                return true;

            if (value is "NO" or "FALSE" or "0")
                return false;

            return null;
        }

        private static decimal? ParseDecimal(string raw)
        {
            raw = raw.Trim();

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

        private static DateOnly? ParseDate(string raw)
        {
            raw = raw.Trim();

            if (string.IsNullOrWhiteSpace(raw) || raw is "N/A" or "--" or "-")
                return null;

            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var oaDate))
            {
                try
                {
                    return DateOnly.FromDateTime(DateTime.FromOADate(oaDate));
                }
                catch
                {
                    return null;
                }
            }

            if (DateTime.TryParse(raw, new CultureInfo("es-DO"), DateTimeStyles.None, out var date))
                return DateOnly.FromDateTime(date);

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return DateOnly.FromDateTime(date);

            return null;
        }

        private static Cell? GetCell(Row row, int columnIndex)
        {
            return row.Elements<Cell>()
                .FirstOrDefault(cell => GetColumnIndex(cell.CellReference?.Value) == columnIndex);
        }

        private static string GetCellValue(Cell? cell, WorkbookPart workbookPart)
        {
            if (cell is null)
                return string.Empty;

            if (cell.DataType?.Value == CellValues.SharedString)
            {
                var sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;
                if (sharedStringTable is null)
                    return cell.InnerText;

                if (int.TryParse(cell.InnerText, out var sharedStringIndex))
                    return sharedStringTable.ElementAt(sharedStringIndex).InnerText;
            }

            if (cell.DataType?.Value == CellValues.InlineString)
                return cell.InlineString?.Text?.Text ?? cell.InnerText;

            if (cell.DataType?.Value == CellValues.Boolean)
                return cell.InnerText == "1" ? "TRUE" : "FALSE";

            return cell.InnerText;
        }

        private static int GetColumnIndex(string? cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return -1;

            var columnIndex = 0;

            foreach (var character in cellReference)
            {
                if (!char.IsLetter(character))
                    break;

                columnIndex = (columnIndex * 26) + (char.ToUpperInvariant(character) - 'A' + 1);
            }

            return columnIndex;
        }
    }
}
