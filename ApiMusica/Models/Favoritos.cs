using System;
using System.Collections.Generic;

namespace ApiMusica.Models;

public partial class Favoritos
{
    public int Codfavorito { get; set; }

    public int? Codcancion { get; set; }

    public int? Codusuario { get; set; }

    public virtual Canciones? oCodcancion { get; set; }

    public virtual Usuario? oCodusuario { get; set; }
}
