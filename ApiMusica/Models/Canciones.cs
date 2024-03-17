using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class Canciones
{
    public int Codcancion { get; set; }

    public string? Nombre { get; set; }

    public int? Codcantante { get; set; }

    public int? Anio { get; set; }

    public int? Estado { get; set; }

    public string? Link { get; set; }

    public string? Rpath { get; set; }

    public virtual Cantantes? oCodcantante { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetPlaylists> DetPlaylists { get; set; } = new List<DetPlaylists>();
    [JsonIgnore]
    public virtual ICollection<Favoritos> Favoritos { get; set; } = new List<Favoritos>();
    [JsonIgnore]
    public virtual ICollection<Reproduciones> Reproduciones { get; set; } = new List<Reproduciones>();
}
