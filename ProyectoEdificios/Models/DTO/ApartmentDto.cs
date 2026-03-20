namespace ProyectoEdificios.Models.DTO
{
    public sealed class ApartmentDto
    {
        public int Id { get; set; }
        public string CodUnidad { get; set; } = string.Empty;
        public string Edificio { get; set; } = string.Empty;
        public string Unidad { get; set; } = string.Empty;
        public decimal? Metraje { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public decimal? Precio { get; set; }
        public decimal? Inicial { get; set; }
        public decimal? InicialDolar { get; set; }
        public decimal? Pagado { get; set; }
        public decimal? Adeudado { get; set; }
        public DateOnly? FechaCompletaInicial { get; set; }
        public DateOnly? FechaInicioVaciados { get; set; }
        public DateOnly? FechaEntregaInspeccion { get; set; }
        public DateOnly? FechaLegal { get; set; }
        public DateOnly? FechaGobierno { get; set; }
        public DateOnly? FechaMicelaneos { get; set; }
        public DateOnly? FechaInspeccion1 { get; set; }
        public DateOnly? FechaInspeccion2 { get; set; }
        public DateOnly? FechaFormaPago { get; set; }
        public bool IniciadoVaciados { get; set; }
        public bool EnInspeccion { get; set; }
        public bool Inspeccion1 { get; set; }
        public bool Inspeccion2 { get; set; }
        public bool Legal { get; set; }
        public bool Gobierno { get; set; }
        public bool Micelaneos { get; set; }
        public bool Titulo { get; set; }
        public string ResponsableLegal { get; set; } = string.Empty;
        public string ResponsableGobierno { get; set; } = string.Empty;
        public string ResponsableMicelaneos { get; set; } = string.Empty;
        public string FormaPago { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public bool Saldo { get; set; }
        public bool Entregada { get; set; }
        public bool DescargadaDGII { get; set; }
    }
}
