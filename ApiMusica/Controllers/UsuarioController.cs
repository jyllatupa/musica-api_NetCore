using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public UsuarioController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Usuario> list = new List<Usuario>();

            try
            {
                list = _dbcontext.Usuarios.ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Obtener/{codusuario:int}")]
        public IActionResult Obtener(int codusuario)
        {
            Usuario oUsuario = _dbcontext.Usuarios.Find(codusuario);

            if (oUsuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            try
            {
                oUsuario = _dbcontext.Usuarios.Where(p => p.Codusuario == codusuario).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = oUsuario });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oUsuario });
            }
        }

        [HttpGet]
        [Route("Filtrar/{nombre:length(1,100)}")]
        public IActionResult Filtrar(string nombre)
        {
            List<Usuario> list = new List<Usuario>();

            list = _dbcontext.Usuarios.Where(x => x.Nombre.Contains(nombre) || x.Nomusuario.Contains(nombre)).ToList();

            if (list == null)
            {
                return BadRequest("Usuario no encontrado");
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
        public IActionResult Guardar([FromBody] Usuario objeto)
        {
            try
            {
                objeto.Fecharegistro = DateTime.Now;
                _dbcontext.Usuarios.Add(objeto);
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
        public IActionResult Editar([FromBody] Usuario objeto)
        {
            Usuario oUsuario = _dbcontext.Usuarios.Find(objeto.Codusuario);

            if (oUsuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            try
            {
                oUsuario.Nomusuario = (objeto.Nomusuario == null) ? oUsuario.Nomusuario : objeto.Nomusuario;
                oUsuario.Nombre = (objeto.Nombre == null) ? oUsuario.Nombre : objeto.Nombre;
                oUsuario.Clave = (objeto.Clave == null) ? oUsuario.Clave : objeto.Clave;
                oUsuario.Email = (objeto.Email == null) ? oUsuario.Email : objeto.Email;
                oUsuario.Fecharegistro = oUsuario.Fecharegistro;
                oUsuario.Estado = (objeto.Estado == null) ? oUsuario.Estado : objeto.Estado;

                _dbcontext.Usuarios.Update(oUsuario);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codusuario:int}")]
        public IActionResult Eliminar(int codusuario)
        {
            Usuario oUsuario = _dbcontext.Usuarios.Find(codusuario);

            if (oUsuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            try
            {
                oUsuario.Estado = 0;
                _dbcontext.Usuarios.Update(oUsuario);
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
