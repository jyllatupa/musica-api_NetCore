using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class Playlists
{
    public int Codplaylist { get; set; }

    public int? Codusuario { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? Fechacreacion { get; set; }

    public virtual Usuario? oCodusuario { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetPlaylists> DetPlaylists { get; set; } = new List<DetPlaylists>();
}
