namespace ApiMusica.Entidad
{
    public class DTO
    {
        public class CantanteGenero
        {
            public int codcantante { get; set; }
            public string? nombre { get; set; }
            public int codgenero { get; set; }
            public string? nomgenero { get; set; }
        }
        public class CancionesWithCantantes
        {
            public int codcancion { get; set; }
            public int codcantante { get; set; }
            public string? nomcancion { get; set; }
            public string? nomcantante { get; set; }
            public int anio { get; set; }
        }

        public class CancionesFavoritas
        {
            public int codfavorito { get; set; }
            public int codcancion { get; set; }
            public int codusuario { get; set; }
            public string? nomcancion { get; set; }
        }

        public class CancionesPlaylist
        {
            public int codplaylist { get; set; }
            public int codusuario { get; set; }
            public string? descripcion { get; set; }
        }

        public class DetPlaylist
        {
            public int coddetplaylist { get; set; }
            public int codplaylist { get; set; }
            public int codcancion { get; set; }
            public string? nomcancion { get; set; }
        }

        public class ReproducionesCanciones
        {
            public int codrepro { get; set; }
            public int codcancion { get; set; }
            public string? nomcancion { get; set; }
            public int codusuario { get; set; }
            public int cantidad { get; set; }
        }
    }
}
