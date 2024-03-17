using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public GeneroController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Generos> list = new List<Generos>();

            try
            {
                list = _dbcontext.Generos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Obtener/{codgenero:int}")]
        public IActionResult Obtener(int codgenero)
        {
            Generos oGenero = _dbcontext.Generos.Find(codgenero);

            if (oGenero == null)
            {
                return BadRequest("Genero de musica no encontrado");
            }

            try
            {
                oGenero = _dbcontext.Generos.Where(p => p.Codgenero == codgenero).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = oGenero });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oGenero });
            }
        }

        [HttpGet]
        [Route("Filtrar/{genero:length(1,100)}")]
        public IActionResult Filtrar(string genero)
        {
            List<Generos> list = new List<Generos>();

            list = _dbcontext.Generos.Where(x => x.Descripcion.Contains(genero)).ToList();

            if (list == null)
            {
                return BadRequest("Genero de musica no encontrado");
            }

            try
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Generos objeto)
        {
            try
            {
                _dbcontext.Generos.Add(objeto);
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
        public IActionResult Editar([FromBody] Generos objeto)
        {
            Generos oGenero = _dbcontext.Generos.Find(objeto.Codgenero);

            if (oGenero == null)
            {
                return BadRequest("Genero de musica no encontrado");
            }

            try
            {
                oGenero.Descripcion = (objeto.Descripcion == null) ? oGenero.Descripcion : objeto.Descripcion;

                _dbcontext.Generos.Update(oGenero);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codgenero:int}")]
        public IActionResult Eliminar(int codgenero)
        {
            Generos oGenero = _dbcontext.Generos.Find(codgenero);

            if (oGenero == null)
            {
                return BadRequest("Genero de musica no encontrado");
            }

            try
            {
                _dbcontext.Generos.Remove(oGenero);
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
