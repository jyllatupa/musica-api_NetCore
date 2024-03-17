using System;
using System.Collections.Generic;

namespace ApiMusica.Models;

public partial class Reproduciones
{
    public int Codrepro { get; set; }

    public int? Codcancion { get; set; }

    public int? Codusuario { get; set; }

    public int? Countrepro { get; set; }

    public virtual Canciones? oCodcancion { get; set; }

    public virtual Usuario? oCodusuario { get; set; }
}
