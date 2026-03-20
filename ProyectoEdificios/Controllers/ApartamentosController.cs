using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using ProyectoEdificios.Models.DTO;
using System.Globalization;

namespace ProyectoEdificios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartamentosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ApartamentosController(IWebHostEnvironment env)
        {
            _env = env;
        }


        private bool? ParseBool(IXLCell cell)
        {
            var value = cell.GetString().Trim().ToUpper();

            if (string.IsNullOrEmpty(value))
                return null;

            if (value == "SI" || value == "YES" || value == "TRUE" || value == "1")
                return true;

            if (value == "NO" || value == "FALSE" || value == "0")
                return false;

            return null;
        }

        private decimal? ParseDecimal(IXLCell cell)
        {
            var raw = cell.GetString().Trim();

            if (string.IsNullOrWhiteSpace(raw))
                return null;

            // Quitar símbolos comunes
            raw = raw.Replace("RD$", "", StringComparison.OrdinalIgnoreCase)
                     .Replace("$", "")
                     .Replace("%", "")
                     .Replace(",", "")   // quitar comas de miles
                     .Trim();

            // reemplazar coma decimal por punto
            raw = raw.Replace(",", ".").Trim();

            if (decimal.TryParse(raw, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var d))
            {
                return d;
            }

            return null;
        }

        private DateTime? ParseDate(IXLCell cell)
        {
            // Si está vacía, null
            if (cell == null || cell.IsEmpty())
                return null;

            // Si Excel la tiene como fecha real
            if (cell.DataType == XLDataType.DateTime)
                return cell.GetDateTime();

            // A veces vienen como número serial de Excel
            if (cell.DataType == XLDataType.Number)
            {
                try
                {
                    double oa = cell.GetDouble();
                    return DateTime.FromOADate(oa);
                }
                catch
                {
                    // ignoramos si no se puede
                }
            }

            // Si viene como texto, intentamos parsear
            var raw = cell.GetString().Trim();
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            // Normalizar cosas raras tipo "N/A", "--", etc.
            if (raw == "N/A" || raw == "--" || raw == "-")
                return null;

            // Intentar con cultura local (RD) y luego Invariant
            if (DateTime.TryParse(raw, new CultureInfo("es-DO"), DateTimeStyles.None, out var dt))
                return dt;

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;

            // Si nada funciona, lo devolvemos como null en vez de explotar
            return null;
        }

        [HttpGet("sheets")]
        public IActionResult GetSheets()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "control2.xlsx");

            if (!System.IO.File.Exists(filePath))
                return NotFound("El archivo de control no existe.");

            using var workbook = new XLWorkbook(filePath);

            var sheetNames = workbook.Worksheets.Select(ws => ws.Name).ToList();

            return Ok(sheetNames);
        }




        //[HttpGet("GetBySheet/{sheetName}")]
        //public IActionResult GetBySheet(string sheetName)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "control2.xlsx");

        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound("El archivo de control no existe.");

        //    using var workbook = new XLWorkbook(filePath);

        //    var worksheet = workbook.Worksheets.FirstOrDefault(ws =>
        //        ws.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));

        //    if (worksheet == null)
        //        return NotFound($"La hoja '{sheetName}' no existe en el archivo.");

        //    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

        //    List<ApartmentDto> lista = new();

        //    foreach (var row in rows)
        //    {
        //        lista.Add(new ApartmentDto
        //        {
        //            CodUnidad = row.Cell(1).GetString(),
        //            Edificio = row.Cell(2).GetString(),
        //            Unidad = row.Cell(3).GetString(),
        //            Metraje = ParseDecimal(row.Cell(4)),
        //            Estado = row.Cell(5).GetString(),
        //            Nombre = row.Cell(6).GetString(),
        //            Telefono = row.Cell(7).GetString(),
        //            Correo = row.Cell(8).GetString(),
        //            Cedula = row.Cell(9).GetString(),
        //            Precio = ParseDecimal(row.Cell(10)),
        //            Inicial = ParseDecimal(row.Cell(11)),
        //            FechaCompletaInicial = ParseDate(row.Cell(12)),
        //            InicialDolar = ParseDecimal(row.Cell(13)),
        //            Pagado = ParseDecimal(row.Cell(14)),
        //            Adeudado = ParseDecimal(row.Cell(15)),
        //            IniciadoVaciados = ParseBool(row.Cell(16)),
        //            FechaInicioVaciados = ParseDate(row.Cell(17)),
        //            EnInspeccion = ParseBool(row.Cell(18)),
        //            FechaEntregaInspeccion = ParseDate(row.Cell(19)),
        //            Legal = ParseBool(row.Cell(20)),
        //            ResponsableLegal = row.Cell(21).GetString(),
        //            FechaLegal = ParseDate(row.Cell(22)),
        //            Gobierno = ParseBool(row.Cell(23)),
        //            ResponsableGobierno = row.Cell(24).GetString(),
        //            FechaGobierno = ParseDate(row.Cell(25)),
        //            Micelaneos = ParseBool(row.Cell(26)),
        //            ResponsableMicelaneos = row.Cell(27).GetString(),
        //            FechaMicelaneos = ParseDate(row.Cell(28)),
        //            Inspeccion1 = ParseBool(row.Cell(29)),
        //            FechaInspeccion1 = ParseDate(row.Cell(30)),
        //            Inspeccion2 = ParseBool(row.Cell(31)),
        //            FechaInspeccion2 = ParseDate(row.Cell(32)),
        //            FormaPago = row.Cell(33).GetString(),
        //            FechaFormaPago = ParseDate(row.Cell(34)),
        //            Banco = row.Cell(35).GetString(),
        //            Saldo = ParseBool(row.Cell(36)),
        //            Entregada = ParseBool(row.Cell(37)),
        //            Titulo = ParseBool(row.Cell(38)),
        //            DescargadaDGII = ParseBool(row.Cell(39)),
        //        });
        //    }

        //    return Ok(lista);
        //}

    }
}
