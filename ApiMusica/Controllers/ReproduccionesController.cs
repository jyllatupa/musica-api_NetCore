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
    public class ReproduccionesController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public ReproduccionesController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<ReproducionesCanciones> list = new List<ReproducionesCanciones>();
            ReproducionesCanciones reproducionescanciones = new ReproducionesCanciones();

            try
            {
                var query = (
                            from c in _dbcontext.Reproduciones
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            select new
                            {
                                codrepro = c.Codrepro,
                                codcancion = c.Codcancion,
                                nomcancion = g.Nombre,
                                codusuario = c.Codusuario,
                                cantidad = c.Countrepro,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    reproducionescanciones = new ReproducionesCanciones();
                    reproducionescanciones.codrepro = x.codrepro;
                    reproducionescanciones.codcancion = x.codcancion.Value;
                    reproducionescanciones.nomcancion = x.nomcancion;
                    reproducionescanciones.codusuario = x.codusuario.Value;
                    reproducionescanciones.cantidad = x.cantidad.Value;
                    list.Add(reproducionescanciones);
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
            Reproduciones oReproduciones = _dbcontext.Reproduciones.Where(f => f.Codcancion == codcancion).FirstOrDefault();

            if (oReproduciones == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            List<ReproducionesCanciones> list = new List<ReproducionesCanciones>();
            ReproducionesCanciones reproducionescanciones = new ReproducionesCanciones();

            try
            {
                var query = (
                            from c in _dbcontext.Reproduciones
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            where c.Codcancion == codcancion
                            select new
                            {
                                codrepro = c.Codrepro,
                                codcancion = c.Codcancion,
                                nomcancion = g.Nombre,
                                codusuario = c.Codusuario,
                                cantidad = c.Countrepro,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    reproducionescanciones = new ReproducionesCanciones();
                    reproducionescanciones.codrepro = x.codrepro;
                    reproducionescanciones.codcancion = x.codcancion.Value;
                    reproducionescanciones.nomcancion = x.nomcancion;
                    reproducionescanciones.codusuario = x.codusuario.Value;
                    reproducionescanciones.cantidad = x.cantidad.Value;
                    list.Add(reproducionescanciones);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oReproduciones });
            }
        }

        [HttpGet]
        [Route("ObtenerByUsu/{codusuario:int}")]
        public IActionResult ObtenerByUsu(int codusuario)
        {
            Reproduciones oReproduciones = _dbcontext.Reproduciones.Where(f => f.Codusuario == codusuario).FirstOrDefault();

            if (oReproduciones == null)
            {
                return BadRequest("Usuario no cuenta con reproduciones");
            }

            List<ReproducionesCanciones> list = new List<ReproducionesCanciones>();
            ReproducionesCanciones reproducionescanciones = new ReproducionesCanciones();

            try
            {
                var query = (
                            from c in _dbcontext.Reproduciones
                            join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                            join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                            where c.Codusuario == codusuario
                            select new
                            {
                                codrepro = c.Codrepro,
                                codcancion = c.Codcancion,
                                nomcancion = g.Nombre,
                                codusuario = c.Codusuario,
                                cantidad = c.Countrepro,
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    reproducionescanciones = new ReproducionesCanciones();
                    reproducionescanciones.codrepro = x.codrepro;
                    reproducionescanciones.codcancion = x.codcancion.Value;
                    reproducionescanciones.nomcancion = x.nomcancion;
                    reproducionescanciones.codusuario = x.codusuario.Value;
                    reproducionescanciones.cantidad = x.cantidad.Value;
                    list.Add(reproducionescanciones);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oReproduciones });
            }
        }

        [HttpGet]
        [Route("Filtrar/{codusuario:int}/{nombre:length(1,100)}")]
        public IActionResult Filtrar(int codusuario, string nombre)
        {
            List<ReproducionesCanciones> list = new List<ReproducionesCanciones>();
            ReproducionesCanciones reproducionescanciones = new ReproducionesCanciones();

            var query = (
                        from c in _dbcontext.Reproduciones
                        join g in _dbcontext.Canciones on c.Codcancion equals g.Codcancion
                        join u in _dbcontext.Usuarios on c.Codusuario equals u.Codusuario
                        where c.Codusuario == codusuario &&  EF.Functions.Like(g.Nombre, "%" + nombre + "%")
                        select new
                        {
                            codrepro = c.Codrepro,
                            codcancion = c.Codcancion,
                            nomcancion = g.Nombre,
                            codusuario = c.Codusuario,
                            cantidad = c.Countrepro,
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
                    reproducionescanciones = new ReproducionesCanciones();
                    reproducionescanciones.codrepro = x.codrepro;
                    reproducionescanciones.codcancion = x.codcancion.Value;
                    reproducionescanciones.nomcancion = x.nomcancion;
                    reproducionescanciones.codusuario = x.codusuario.Value;
                    reproducionescanciones.cantidad = x.cantidad.Value;
                    list.Add(reproducionescanciones);
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
        public IActionResult Guardar([FromBody] Reproduciones objeto)
        {
            try
            {
                _dbcontext.Reproduciones.Add(objeto);
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
        public IActionResult Editar([FromBody] Reproduciones objeto)
        {
            Reproduciones oReproduciones = _dbcontext.Reproduciones.Find(objeto.Codrepro);

            if (oReproduciones == null)
            {
                return BadRequest("Reproducion no encontrada");
            }

            try
            {
                oReproduciones.Codusuario = (objeto.Codusuario == null) ? oReproduciones.Codusuario : objeto.Codusuario;
                oReproduciones.Codcancion = (objeto.Codcancion == null) ? oReproduciones.Codcancion : objeto.Codcancion;
                oReproduciones.Countrepro = (objeto.Countrepro == null) ? oReproduciones.Countrepro : objeto.Countrepro;

                _dbcontext.Reproduciones.Update(oReproduciones);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codrepro:int}")]
        public IActionResult Eliminar(int codrepro)
        {
            Reproduciones oReproduciones = _dbcontext.Reproduciones.Find(codrepro);

            if (oReproduciones == null)
            {
                return BadRequest("Reproducion no encontrada");
            }

            try
            {
                _dbcontext.Reproduciones.Remove(oReproduciones);
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

