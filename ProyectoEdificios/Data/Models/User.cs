using System;
using System.Collections.Generic;

namespace ProyectoEdificios.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; }

    public string Clave { get; set; }

    public string? Salt { get; set; }
}
