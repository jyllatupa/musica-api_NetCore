using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;
using static ApiMusica.Entidad.DTO;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public PlaylistsController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<CancionesPlaylist> list = new List<CancionesPlaylist>();
            CancionesPlaylist cancionesPlaylist = new CancionesPlaylist();

            try
            {
                var query = (
                            from c in _dbcontext.Playlists
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            select new
                            {
                                codplaylist = c.Codplaylist,
                                codusuario = c.Codusuario,
                                descripcion = c.Descripcion,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesPlaylist = new CancionesPlaylist();
                    cancionesPlaylist.codplaylist = x.codplaylist;
                    cancionesPlaylist.codusuario = x.codusuario.Value;
                    cancionesPlaylist.descripcion = x.descripcion;
                    list.Add(cancionesPlaylist);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Obtener/{codplaylist:int}")]
        public IActionResult Obtener(int codplaylist)
        {
            var query = (
                            from c in _dbcontext.Playlists
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            where c.Codplaylist == codplaylist
                            select new
                            {
                                codplaylist = c.Codplaylist,
                                codusuario = c.Codusuario,
                                descripcion = c.Descripcion,
                            }
                            ).ToList();

            List<CancionesPlaylist> list = new List<CancionesPlaylist>();
            CancionesPlaylist cancionesPlaylist = new CancionesPlaylist();

            if (query == null)
            {
                return BadRequest("PlayList no encontrada");
            }

            try
            {
                foreach (var x in query)
                {
                    cancionesPlaylist = new CancionesPlaylist();
                    cancionesPlaylist.codplaylist = x.codplaylist;
                    cancionesPlaylist.codusuario = x.codusuario.Value;
                    cancionesPlaylist.descripcion = x.descripcion;
                    list.Add(cancionesPlaylist);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("ObtenerByUsu/{codusuario:int}")]
        public IActionResult ObtenerByUsu(int codusuario)
        {
            Playlists oPlaylists = _dbcontext.Playlists.Where(f => f.Codusuario == codusuario).FirstOrDefault();

            if (oPlaylists == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            List<CancionesPlaylist> list = new List<CancionesPlaylist>();
            CancionesPlaylist cancionesPlaylist = new CancionesPlaylist();

            try
            {
                var query = (
                            from c in _dbcontext.Playlists
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            where c.Codusuario == codusuario
                            select new
                            {
                                codplaylist = c.Codplaylist,
                                codusuario = c.Codusuario,
                                descripcion = c.Descripcion,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesPlaylist = new CancionesPlaylist();
                    cancionesPlaylist.codplaylist = x.codplaylist;
                    cancionesPlaylist.codusuario = x.codusuario.Value;
                    cancionesPlaylist.descripcion = x.descripcion;
                    list.Add(cancionesPlaylist);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Filtrar/{codusuario:int}/{nombre:length(1,100)}")]
        public IActionResult Filtrar(int codusuario, string nombre)
        {
            List<CancionesPlaylist> list = new List<CancionesPlaylist>();
            CancionesPlaylist cancionesPlaylist = new CancionesPlaylist();

            var query = (
                        from c in _dbcontext.Playlists
                        join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                        where c.Codusuario == codusuario && EF.Functions.Like(c.Descripcion, "%" + nombre + "%")
                        select new
                        {
                            codplaylist = c.Codplaylist,
                            codusuario = c.Codusuario,
                            descripcion = c.Descripcion,
                        }
                        ).ToList();

            if (query == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                foreach (var x in query)
                {
                    cancionesPlaylist = new CancionesPlaylist();
                    cancionesPlaylist.codplaylist = x.codplaylist;
                    cancionesPlaylist.codusuario = x.codusuario.Value;
                    cancionesPlaylist.descripcion = x.descripcion;
                    list.Add(cancionesPlaylist);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Playlists objeto)
        {
            try
            {
                _dbcontext.Playlists.Add(objeto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Playlists objeto)
        {
            Playlists oPlaylists = _dbcontext.Playlists.Find(objeto.Codplaylist);

            if (oPlaylists == null)
            {
                return BadRequest("PlayList no encontrado");
            }

            try
            {
                oPlaylists.Codusuario = (objeto.Codusuario == null) ? oPlaylists.Codusuario : objeto.Codusuario;
                oPlaylists.Descripcion = (objeto.Descripcion == null) ? oPlaylists.Descripcion : objeto.Descripcion;

                _dbcontext.Playlists.Update(oPlaylists);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codplaylist:int}")]
        public IActionResult Eliminar(int codplaylist)
        {
            Playlists oPlaylists = _dbcontext.Playlists.Find(codplaylist);

            if (oPlaylists == null)
            {
                return BadRequest("PlayList no encontrado");
            }

            try
            {
                _dbcontext.Playlists.Remove(oPlaylists);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
