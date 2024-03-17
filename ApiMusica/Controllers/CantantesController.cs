using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMusica.Models;

using Microsoft.AspNetCore.Cors;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static ApiMusica.Entidad.DTO;
using System.Collections.Generic;

namespace ApiMusica.Controllers
{
    //aplicando las reglas de CORS dentro de nuestro controlador Genero
    [EnableCors("ReglasCORS")]
    [Route("api/[controller]")]
    [ApiController]
    public class CantantesController : ControllerBase
    {
        public readonly MusicaContext _dbcontext;

        public CantantesController(MusicaContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<CantanteGenero> list = new List<CantanteGenero>();
            CantanteGenero cantanteGenero = new CantanteGenero();

            try
            {
                var query = (
                            from c in _dbcontext.Cantantes
                            join g in _dbcontext.Generos on c.Codgenero equals g.Codgenero
                            select new
                            {
                                codcantante = c.Codcantante,
                                nombre = c.Nombre,
                                codgenero = c.Codgenero,
                                nomgenero = g.Descripcion
                            }
                            ).ToList();

                foreach (var c in query)
                {
                    cantanteGenero = new CantanteGenero();
                    cantanteGenero.codcantante = c.codcantante;
                    cantanteGenero.nombre = c.nombre;
                    cantanteGenero.codgenero = c.codgenero.Value;
                    cantanteGenero.nomgenero = c.nomgenero;
                    list.Add(cantanteGenero);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = list });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = list });
            }
        }

        [HttpGet]
        [Route("Obtener/{codcantante:int}")]
        public IActionResult Obtener(int codcantante)
        {
            CantanteGenero cantanteGenero = new CantanteGenero();
            List<CantanteGenero> list = new List<CantanteGenero>();
            Cantantes oCantantes = _dbcontext.Cantantes.Find(codcantante);

            if (oCantantes == null)
            {
                return BadRequest("Cantante no encontrado");
            }

            try
            {
                var query = (
                            from c in _dbcontext.Cantantes
                            join g in _dbcontext.Generos on c.Codgenero equals g.Codgenero
                            where c.Codcantante == codcantante
                            select new
                            {
                                codcantante = c.Codcantante,
                                nombre = c.Nombre,
                                codgenero = c.Codgenero,
                                nomgenero = g.Descripcion
                            }
                            ).ToList();

                foreach (var c in query)
                {
                    cantanteGenero = new CantanteGenero();
                    cantanteGenero.codcantante = c.codcantante;
                    cantanteGenero.nombre = c.nombre;
                    cantanteGenero.codgenero = c.codgenero.Value;
                    cantanteGenero.nomgenero = c.nomgenero;
                    list.Add(cantanteGenero);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = oCantantes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oCantantes });
            }
        }

        [HttpGet]
        [Route("Filtrar/{nombre:length(1,100)}")]
        public IActionResult Filtrar(string nombre)
        {
            CantanteGenero cantanteGenero = new CantanteGenero();
            List<CantanteGenero> list = new List<CantanteGenero>();

            var query = (
                        from c in _dbcontext.Cantantes
                        join g in _dbcontext.Generos on c.Codgenero equals g.Codgenero
                        //where EF.Functions.Like(c.Nombre, "%" + nombre + "%") || EF.Functions.Like(g.Descripcion, "%" + nombre + "%")
                        where EF.Functions.Like(c.Nombre, "%" + nombre + "%")
                        select new
                        {
                            codcantante = c.Codcantante,
                            nombre = c.Nombre,
                            codgenero = c.Codgenero,
                            nomgenero = g.Descripcion
                        }
                        ).ToList();

            if (list == null)
            {
                return BadRequest("Cantante no encontrado");
            }

            try
            {
                foreach (var c in query)
                {
                    cantanteGenero = new CantanteGenero();
                    cantanteGenero.codcantante = c.codcantante;
                    cantanteGenero.nombre = c.nombre;
                    cantanteGenero.codgenero = c.codgenero.Value;
                    cantanteGenero.nomgenero = c.nomgenero;
                    list.Add(cantanteGenero);
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
        public IActionResult Guardar([FromBody] Cantantes objeto)
        {
            try
            {
                _dbcontext.Cantantes.Add(objeto);
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
        public IActionResult Editar([FromBody] Cantantes objeto)
        {
            Cantantes oCantantes = _dbcontext.Cantantes.Find(objeto.Codcantante);

            if (oCantantes == null)
            {
                return BadRequest("Cantante no encontrado");
            }

            try
            {
                oCantantes.Nombre = (objeto.Nombre == null) ? oCantantes.Nombre : objeto.Nombre;
                oCantantes.Codgenero = (objeto.Codgenero == null) ? oCantantes.Codgenero : objeto.Codgenero;

                _dbcontext.Cantantes.Update(oCantantes);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{codcantante:int}")]
        public IActionResult Eliminar(int codcantante)
        {
            Cantantes oCantantes = _dbcontext.Cantantes.Find(codcantante);

            if (oCantantes == null)
            {
                return BadRequest("Cantante no encontrado");
            }

            try
            {
                _dbcontext.Cantantes.Remove(oCantantes);
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
