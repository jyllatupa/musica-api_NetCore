using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class Cantantes
{
    public int Codcantante { get; set; }

    public string? Nombre { get; set; }

    public int? Codgenero { get; set; }
    [JsonIgnore]
    public virtual ICollection<Canciones> Canciones { get; set; } = new List<Canciones>();

    public virtual Generos? oCodgenero { get; set; }
}
