using API_REST.Comunes;
using API_REST.Models;
using API_REST.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private Contexto _contexto;
        public UsuarioController(Contexto contexto)
        {
            _contexto= contexto;
        }
        [HttpPost("/registro")]
        public async Task<ActionResult> Registro(RegistroRequest request)
        {
            try
            {
                if (_contexto.Usuarios.FirstOrDefault(x => x.Email == request.Email.ToLower()) != null)
                {
                    throw new Exception("El email ya esta registrado!");
                }
                var usuario = new Usuarios() { Nombres = request.Nombres, Apellidos = request.Apellidos, Email = request.Email.ToLower(), Password = request.Password };
                await _contexto.Usuarios.AddAsync(usuario);
                await _contexto.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest(new Error(e.Message));
            }
        }
        
        [HttpPost("/login")]
        public ActionResult Login(LoginRequest request)
        {
            try
            {
                var usuario = _contexto.Usuarios.FirstOrDefault(x => x.Email == request.Email.ToLower() && x.Password == request.Password);
                if (usuario == null)
                {
                    throw new Exception("Credenciales incorrectas!");
                }
                usuario.Password = null;
                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest(new Error(e.Message));
            }
        }
    }
}
