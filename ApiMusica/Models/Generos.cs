using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class Generos
{
    public int Codgenero { get; set; }

    public string? Descripcion { get; set; }
    [JsonIgnore]
    public virtual ICollection<Cantantes> Cantantes { get; set; } = new List<Cantantes>();
}
