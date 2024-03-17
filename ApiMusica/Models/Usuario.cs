using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class Usuario
{
    public int Codusuario { get; set; }

    public string? Nomusuario { get; set; }

    public string? Nombre { get; set; }

    public string? Clave { get; set; }

    public string? Email { get; set; }

    public int? Estado { get; set; }

    public DateTime Fecharegistro { get; set; }
    [JsonIgnore]
    public virtual ICollection<Favoritos> Favoritos { get; set; } = new List<Favoritos>();
    [JsonIgnore]
    public virtual ICollection<Playlists> Playlists { get; set; } = new List<Playlists>();
    [JsonIgnore]
    public virtual ICollection<Reproduciones> Reproduciones { get; set; } = new List<Reproduciones>();
}
