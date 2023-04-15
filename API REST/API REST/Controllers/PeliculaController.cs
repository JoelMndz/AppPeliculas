using API_REST.Comunes;
using API_REST.Models;
using API_REST.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using API_REST.Helpers;

namespace API_REST.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private Contexto _contexto;

        public PeliculaController(Contexto contexto)
        {
            _contexto= contexto;
        }

        [HttpGet("{idUsuario:int}")]
        public async Task<ActionResult> ObtenerTodoPorUsuario(int idUsuario)
        {
            try
            {
                var listaDTO = new List<PeliculaDTO>();
                var lista = await _contexto.Peliculas
                    .Where(x => x.IdUsuario == idUsuario)
                    .Include(x => x.IdGeneroNavigation)
                    .Include(x => x.IdUsuarioNavigation)
                    .ToArrayAsync();
                foreach (var i in lista)
                {
                    listaDTO.Add(new PeliculaDTO(i.Id, i.Titulo, i.Director, i.IdGenero ?? 0, i.IdGeneroNavigation.Descripcion, i.Anio ?? 0, i.FotoUrl));
                }
                return Ok(listaDTO);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Crear(PeliculaCrearRequest request)
        {
            try
            {
                var genero = _contexto.Generos.FirstOrDefault(x => x.Id == request.IdGenero);
                
                if (genero == null)
                    throw new Exception("El id del Genero no existe!");
                var usuario = _contexto.Usuarios.FirstOrDefault(x => x.Id == request.IdUsuario);
                if(usuario == null)
                    throw new Exception("El id del Usuario no existe!");
                var rutaImagen = await SubirImagen.SubirImagenLocalmente(request.FotoBase64);
                var pelicula = new Peliculas() { 
                    Titulo = request.Titulo, 
                    Anio = request.Anio, 
                    Director=request.Director, 
                    FotoUrl= rutaImagen, 
                    IdGenero = request.IdGenero,
                    IdUsuario = request.IdUsuario,
                    IdGeneroNavigation = genero,
                    IdUsuarioNavigation= usuario
                };
                await _contexto.Peliculas.AddAsync(pelicula);
                await _contexto.SaveChangesAsync();
                return Ok(JsonSerializer.Serialize(pelicula, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }));
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }

        [HttpPatch]
        public async Task<ActionResult> Actualizar(PeliculaActualizarRequest request)
        {
            try
            {
                var pelicula = _contexto.Peliculas.FirstOrDefault(x => x.Id == request.Id);
                if (pelicula == null)
                    throw new Exception("El id de la pelicula no existe!");

                var genero = _contexto.Generos.FirstOrDefault(x => x.Id == request.IdGenero);
                if (genero == null)
                    throw new Exception("El id del Genero no existe!");

                pelicula.IdGeneroNavigation = genero;
                pelicula.IdGenero = request.IdGenero;
                pelicula.Titulo = request.Titulo;
                pelicula.Director = request.Director;
                pelicula.Anio = request.Anio;
                _contexto.Peliculas.Update(pelicula);
                await _contexto.SaveChangesAsync();

                return Ok(JsonSerializer.Serialize(pelicula, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true
                    }));
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
                var pelicula = _contexto.Peliculas.FirstOrDefault(x => x.Id == id);
                if (pelicula == null)
                    throw new Exception("El id no existe!");

                _contexto.Peliculas.Remove(pelicula);
                await _contexto.SaveChangesAsync();
                return Ok(id);
            }
            catch (Exception error)
            {
                return BadRequest(new Error(error.Message));
            }
        }

        public record PeliculaDTO(int Id, string Titulo, string Director, int IdGenero, string Genero, int Anio, string FotoUrl);
    }
}
