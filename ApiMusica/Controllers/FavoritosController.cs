using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static ApiMusica.Entidad.DTO;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public FavoritosController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<CancionesFavoritas> list = new List<CancionesFavoritas>();
            CancionesFavoritas cancionesFavoritas = new CancionesFavoritas();
            try
            {
                var query = (
                            from c in _dbcontext.Favoritos
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            select new
                            {
                                codfavorito = c.Codfavorito,
                                codcancion = c.Codcancion,
                                codusuario = c.Codusuario,
                                NomCancion = g.Nombre,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesFavoritas = new CancionesFavoritas();
                    cancionesFavoritas.codfavorito = x.codfavorito;
                    cancionesFavoritas.codcancion = x.codcancion.Value;
                    cancionesFavoritas.codusuario = x.codusuario.Value;
                    cancionesFavoritas.nomcancion = x.NomCancion;
                    list.Add(cancionesFavoritas);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Obtener/{codcancion:int}")]
        public IActionResult Obtener(int codcancion)
        {
            Favoritos oFavoritos = _dbcontext.Favoritos.Where(f => f.Codcancion == codcancion).FirstOrDefault();

            if (oFavoritos == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            List<CancionesFavoritas> list = new List<CancionesFavoritas>();
            CancionesFavoritas cancionesFavoritas = new CancionesFavoritas();

            try
            {
                var query = (
                            from c in _dbcontext.Favoritos
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            where c.Codcancion == codcancion
                            select new
                            {
                                codfavorito = c.Codfavorito,
                                codcancion = c.Codcancion,
                                NomCancion = g.Nombre,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesFavoritas = new CancionesFavoritas();
                    cancionesFavoritas.codfavorito = x.codfavorito;
                    cancionesFavoritas.codcancion = x.codcancion.Value;
                    cancionesFavoritas.nomcancion = x.NomCancion;
                    list.Add(cancionesFavoritas);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oFavoritos });
            }
        }

        [HttpGet]
        [Route("ObtenerByUsu/{codusuario:int}")]
        public IActionResult ObtenerByUsu(int codusuario)
        {

            Favoritos oFavoritos = _dbcontext.Favoritos.Where(f => f.Codusuario == codusuario).FirstOrDefault();

            if (oFavoritos == null)
            {
                return BadRequest("Este usuario no tiene canciones favoritas");
            }

            List<CancionesFavoritas> list = new List<CancionesFavoritas>();
            CancionesFavoritas cancionesFavoritas = new CancionesFavoritas();

            try
            {
                var query = (
                            from c in _dbcontext.Favoritos
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            where c.Codusuario == codusuario
                            select new
                            {
                                codfavorito = c.Codfavorito,
                                codcancion = c.Codcancion,
                                NomCancion = g.Nombre,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesFavoritas = new CancionesFavoritas();
                    cancionesFavoritas.codfavorito = x.codfavorito;
                    cancionesFavoritas.codcancion = x.codcancion.Value;
                    cancionesFavoritas.nomcancion = x.NomCancion;
                    list.Add(cancionesFavoritas);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oFavoritos });
            }
        }

        [HttpGet]
        [Route("Filtrar/{nombre:length(1,100)}")]
        public IActionResult Filtrar(string nombre)
        {
            List<CancionesFavoritas> list = new List<CancionesFavoritas>();
            CancionesFavoritas cancionesFavoritas = new CancionesFavoritas();

            var query = (
                        from c in _dbcontext.Favoritos
                        join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                        where EF.Functions.Like(g.Nombre, "%" + nombre + "%")
                        select new
                        {
                            codfavorito = c.Codfavorito,
                            codcancion = c.Codcancion,
                            NomCancion = g.Nombre,
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
                    cancionesFavoritas = new CancionesFavoritas();
                    cancionesFavoritas.codfavorito = x.codfavorito;
                    cancionesFavoritas.codcancion = x.codcancion.Value;
                    cancionesFavoritas.nomcancion = x.NomCancion;
                    list.Add(cancionesFavoritas);
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
        public IActionResult Guardar([FromBody] Favoritos objeto)
        {
            try
            {
                _dbcontext.Favoritos.Add(objeto);
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
        public IActionResult Editar([FromBody] Favoritos objeto)
        {
            Favoritos oFavoritos = _dbcontext.Favoritos.Find(objeto.Codfavorito);

            if (oFavoritos == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                oFavoritos.Codcancion = (objeto.Codcancion == null) ? oFavoritos.Codcancion : objeto.Codcancion;
                oFavoritos.Codusuario = (objeto.Codusuario == null) ? oFavoritos.Codusuario : objeto.Codusuario;

                _dbcontext.Favoritos.Update(oFavoritos);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codfavorito:int}")]
        public IActionResult Eliminar(int codfavorito)
        {
            Favoritos oFavoritos = _dbcontext.Favoritos.Find(codfavorito);

            if (oFavoritos == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                _dbcontext.Favoritos.Remove(oFavoritos);
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