namespace ProyectoEdificios.Models.DTO
{
    public sealed class ProjectApartmentsStatsDto
    {
        public string ProjectId { get; set; } = default!;
        public int Edificios { get; set; }
        public int Vendida { get; set; }
        public int TotalUnidades { get; set; }
        public int UnidadesEntregadas { get; set; }
        public int UnidadesConSaldo { get; set; }
        public int UnidadesEnInspeccion { get; set; }
        public int DisponiblesObservacion { get; set; }
    }
}
