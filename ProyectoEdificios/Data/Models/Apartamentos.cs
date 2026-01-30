using System;

namespace ProyectoEdificios.Data.Models
{
    public partial class Apartamentos
    {
        public int Id { get; set; }
        public string? CodUnidad { get; set; }
        public string? Edificio { get; set; }
        public string? Unidad { get; set; }
        public decimal? Metraje { get; set; }
        public string? Estado { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Cedula { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Inicial { get; set; }
        public DateTime? FechaCompletaInicial { get; set; }
        public decimal? InicialDolar { get; set; }
        public decimal? Pagado { get; set; }
        public decimal? Adeudado { get; set; }
        public bool? IniciadoVaciados { get; set; }
        public DateTime? FechaInicioVaciados { get; set; }
        public bool? EnInspeccion { get; set; }
        public DateTime? FechaEntregaInspeccion { get; set; }
        public bool? Legal { get; set; }
        public string? ResponsableLegal { get; set; }
        public DateTime? FechaLegal { get; set; }
        public bool? Gobierno { get; set; }
        public string? ResponsableGobierno { get; set; }
        public DateTime? FechaGobierno { get; set; }
        public bool? Micelaneos { get; set; }
        public string? ResponsableMicelaneos { get; set; }
        public DateTime? FechaMicelaneos { get; set; }
        public bool? Inspeccion1 { get; set; }
        public DateTime? FechaInspeccion1 { get; set; }
        public bool? Inspeccion2 { get; set; }
        public DateTime? FechaInspeccion2 { get; set; }
        public string? FormaPago { get; set; }
        public DateTime? FechaFormaPago { get; set; }
        public string? Banco { get; set; }
        public bool? Saldo { get; set; }
        public bool? Entregada { get; set; }
        public bool? Titulo { get; set; }
        public bool? DescargadaDGII { get; set; }
    }
}
