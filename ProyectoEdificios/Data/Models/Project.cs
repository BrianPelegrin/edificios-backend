using System;
using System.Collections.Generic;

namespace ProyectoEdificios.Data.Models;

public partial class Project
{
    public string Id { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Provincia { get; set; }

    public string? Municipio { get; set; }

    public string? ImagenPlano { get; set; }

    public virtual ICollection<Edificio> Edificios { get; set; } = new List<Edificio>();
}
