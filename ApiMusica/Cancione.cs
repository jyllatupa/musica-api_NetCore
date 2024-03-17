using System;
using System.Collections.Generic;

namespace ApiMusica;

public partial class Cancione
{
    public int Codcancion { get; set; }

    public string? Nombre { get; set; }

    public int? Codcantante { get; set; }

    public int? Anio { get; set; }

    public int? Estado { get; set; }

    public string? Link { get; set; }

    public string? Rpath { get; set; }
}
