using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;
using static ApiMusica.Entidad.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class DetPlaylistController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public DetPlaylistController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar/{codusuario:int}")]
        public IActionResult Listar(int codusuario)
        {
            List<DetPlaylist> list = new List<DetPlaylist>();
            DetPlaylist detPlaylist = new DetPlaylist();

            try
            {
                var query = (
                            from c in _dbcontext.DetPlaylists
                            join p in _dbcontext.Playlists on c.Codplaylist equals p.Codplaylist
                            join ca in _dbcontext.Canciones on c.Codcancion equals ca.Codcancion
                            where p.Codusuario == codusuario
                            select new
                            {
                                coddetplaylist = c.Coddetplay,
                                codplaylist = c.Codplaylist,
                                codcancion = c.Codcancion,
                                nomcancion = ca.Nombre,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    detPlaylist = new DetPlaylist();
                    detPlaylist.coddetplaylist = x.coddetplaylist;
                    detPlaylist.codplaylist = x.codplaylist.Value;
                    detPlaylist.codcancion = x.codcancion.Value;
                    detPlaylist.nomcancion = x.nomcancion;
                    list.Add(detPlaylist);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Filtrar/{codusuario:int}/{nomcancion:length(1,100)}")]
        public IActionResult Filtrar(int codusuario, string nomcancion)
        {
            List<DetPlaylist> list = new List<DetPlaylist>();
            DetPlaylist detPlaylist = new DetPlaylist();

            var query = (
                        from c in _dbcontext.DetPlaylists
                        join p in _dbcontext.Playlists on c.Codplaylist equals p.Codplaylist
                        join ca in _dbcontext.Canciones on c.Codcancion equals ca.Codcancion
                        where p.Codusuario == codusuario && EF.Functions.Like(ca.Nombre, "%" + nomcancion + "%")
                        select new
                        {
                            coddetplaylist = c.Coddetplay,
                            codplaylist = c.Codplaylist,
                            codcancion = c.Codcancion,
                            nomcancion = ca.Nombre,
                        }
                        ).ToList();

            if (query == null || query.Count==0)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                foreach (var x in query)
                {
                    detPlaylist = new DetPlaylist();
                    detPlaylist.coddetplaylist = x.coddetplaylist;
                    detPlaylist.codplaylist = x.codplaylist.Value;
                    detPlaylist.codcancion = x.codcancion.Value;
                    detPlaylist.nomcancion = x.nomcancion;
                    list.Add(detPlaylist);
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
        public IActionResult Guardar([FromBody] DetPlaylists objeto)
        {
            try
            {
                _dbcontext.DetPlaylists.Add(objeto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codcancion:int}")]
        public IActionResult Eliminar(int codcancion)
        {
            DetPlaylists oDetPlaylists = _dbcontext.DetPlaylists.Where(d => d.Codcancion == codcancion).FirstOrDefault();

            if (oDetPlaylists == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                _dbcontext.DetPlaylists.Remove(oDetPlaylists);
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
