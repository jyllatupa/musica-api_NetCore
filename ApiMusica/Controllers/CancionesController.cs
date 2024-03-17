using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;
using ApiMusica.Entidad;

using Microsoft.AspNetCore.Cors;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class CancionesController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public CancionesController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<DTO.CancionesWithCantantes> listaInicial = new List<DTO.CancionesWithCantantes>();
            DTO.CancionesWithCantantes cancionesWith = new DTO.CancionesWithCantantes();

            try
            {
                var query = (
                            from c in _dbcontext.Canciones
                            join g in _dbcontext.Cantantes on c.Codcantante equals g.Codcantante
                            select new
                            {
                                codcancion = c.Codcancion,
                                codcantante = c.Codcantante,
                                NomCancion = c.Nombre,
                                NomCantante = g.Nombre,
                                anio = c.Anio
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesWith = new DTO.CancionesWithCantantes();
                    cancionesWith.codcancion = x.codcancion;
                    cancionesWith.codcantante = x.codcantante.Value;
                    cancionesWith.nomcancion = x.NomCancion;
                    cancionesWith.nomcantante = x.NomCantante;
                    cancionesWith.anio = x.anio.Value;
                    listaInicial.Add(cancionesWith);
                }

                //list = query;

                //list = _dbcontext.Canciones.Include(c => c.oCodcantante).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = listaInicial });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = listaInicial });
            }
        }

        [HttpGet]
        [Route("Obtener/{codcancion:int}")]
        public IActionResult Obtener(int codcancion)
        {
            Canciones oCanciones = _dbcontext.Canciones.Find(codcancion);
            List<DTO.CancionesWithCantantes> list = new List<DTO.CancionesWithCantantes>();
            DTO.CancionesWithCantantes cancionesWith = new DTO.CancionesWithCantantes();

            if (oCanciones == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                var query = (
                            from c in _dbcontext.Canciones
                            join g in _dbcontext.Cantantes on c.Codcantante equals g.Codcantante
                            where c.Codcancion == codcancion
                            select new
                            {
                                codcancion = c.Codcancion,
                                codcantante = c.Codcantante,
                                NomCancion = c.Nombre,
                                NomCantante = g.Nombre,
                                anio = c.Anio
                            }
                            ).ToList();

                foreach (var x in query)
                {
                    cancionesWith = new DTO.CancionesWithCantantes();
                    cancionesWith.codcancion = x.codcancion;
                    cancionesWith.codcantante = x.codcantante.Value;
                    cancionesWith.nomcancion = x.NomCancion;
                    cancionesWith.nomcantante = x.NomCantante;
                    cancionesWith.anio = x.anio.Value;
                    list.Add(cancionesWith);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Filtrar/{nombre:length(1,100)}")]
        public IActionResult Filtrar(string nombre)
        {
            List<DTO.CancionesWithCantantes> list = new List<DTO.CancionesWithCantantes>();
            DTO.CancionesWithCantantes cancionesWith = new DTO.CancionesWithCantantes();

            var query = (
                        from c in _dbcontext.Canciones
                        join g in _dbcontext.Cantantes on c.Codcantante equals g.Codcantante
                        where EF.Functions.Like(c.Nombre, "%" + nombre + "%")
                        //where EF.Functions.Like(c.Nombre, "%" + nombre + "%") || EF.Functions.Like(g.Nombre, "%" + nombre + "%")
                        select new
                        {
                            codcancion = c.Codcancion,
                            codcantante = c.Codcantante,
                            NomCancion = c.Nombre,
                            NomCantante = g.Nombre,
                            anio = c.Anio
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
                    cancionesWith = new DTO.CancionesWithCantantes();
                    cancionesWith.codcancion = x.codcancion;
                    cancionesWith.codcantante = x.codcantante.Value;
                    cancionesWith.nomcancion = x.NomCancion;
                    cancionesWith.nomcantante = x.NomCantante;
                    cancionesWith.anio = x.anio.Value;
                    list.Add(cancionesWith);
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
        public IActionResult Guardar([FromBody] Canciones objeto)
        {
            try
            {
                _dbcontext.Canciones.Add(objeto);
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
        public IActionResult Editar([FromBody] Canciones objeto)
        {
            Canciones oCanciones = _dbcontext.Canciones.Find(objeto.Codcancion);

            if (oCanciones == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                oCanciones.Nombre = (objeto.Nombre == null) ? oCanciones.Nombre : objeto.Nombre;
                oCanciones.Codcantante = (objeto.Codcantante == null) ? oCanciones.Codcantante : objeto.Codcantante;
                oCanciones.Anio = (objeto.Anio == null) ? oCanciones.Anio : objeto.Anio;
                oCanciones.Estado = (objeto.Estado == null) ? oCanciones.Estado : objeto.Estado;

                _dbcontext.Canciones.Update(oCanciones);
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
            Canciones oCanciones = _dbcontext.Canciones.Find(codcancion);

            if (oCanciones == null)
            {
                return BadRequest("Cancion no encontrada");
            }

            try
            {
                oCanciones.Estado = 0;
                _dbcontext.Canciones.Update(oCanciones);
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
