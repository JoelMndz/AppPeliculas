using API_REST.Comunes;
using API_REST.Models;
using API_REST.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_REST.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private Contexto _contexto;
        public GeneroController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodo()
        {
            try
            {
                var lista = await _contexto.Generos.ToArrayAsync();
                return Ok(lista);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Crear(GeneroCrearRequest request)
        {
            try
            {
                var genero = new Generos() { Descripcion = request.Descripcion };
                await _contexto.Generos.AddAsync(genero);
                await _contexto.SaveChangesAsync();
                return Ok(genero);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }


        [HttpPatch]
        public async Task<ActionResult> Actualizar(GeneroActualizarRequest request)
        {
            try
            {
                var genero = _contexto.Generos.FirstOrDefault(x => x.Id == request.Id);
                if (genero == null)
                    throw new Exception("El id no existe!");
                genero.Descripcion = request.Descripcion;
                _contexto.Generos.Update(genero);
                await _contexto.SaveChangesAsync();
                return Ok(genero);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                var genero = _contexto.Generos.FirstOrDefault(x => x.Id == id);
                if (genero == null)
                    throw new Exception("El id no existe!");
                
                _contexto.Generos.Remove(genero);
                await _contexto.SaveChangesAsync();
                return Ok(genero);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }

    }
}
