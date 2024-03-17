using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiMusica.Models;

public partial class DetPlaylists
{
    public int Coddetplay { get; set; }

    public int? Codplaylist { get; set; }

    public int? Codcancion { get; set; }

    public virtual Canciones? oCodcancion { get; set; }
    [JsonIgnore]
    public virtual Playlists? CodplaylistNavigation { get; set; }
}
