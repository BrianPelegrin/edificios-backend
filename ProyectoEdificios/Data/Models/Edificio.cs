using System;
using System.Collections.Generic;

namespace ProyectoEdificios.Data.Models;

public partial class Edificio
{
    public string ProjectId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Units { get; set; }

    public string? UnitKeys { get; set; }

    public int? TotalUnits { get; set; }

    public int? Columns { get; set; }

    public int? Rowspercolumn { get; set; }

    public double? X { get; set; }

    public double? Y { get; set; }

    public double? Z { get; set; }

    public int? Rotation { get; set; }

    public int? Color { get; set; }

    public int? FootprintScale { get; set; }

    public int? CubeScale { get; set; }




    public virtual Project? Project { get; set; } = null!;
}
