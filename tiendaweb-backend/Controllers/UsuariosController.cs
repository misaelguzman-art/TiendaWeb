using Microsoft.AspNetCore.Mvc;
using tiendaweb_backend.Negocio;

namespace tiendaweb_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly GestorUsuarios _gestorUsuarios;

    public UsuariosController(GestorUsuarios gestorUsuarios)
    {
        _gestorUsuarios = gestorUsuarios;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var usuario = _gestorUsuarios.Autenticar(request.User, request.Contrasena);
        
        if (usuario == null)
            return Unauthorized("Usuario o contrasena incorrectos.");

        return Ok(new
        {
            id = usuario.Id,
            user = usuario.User,
            rol = usuario.ObtenerRol()
        });
    }

    [HttpPost("registro")]
    public IActionResult Registro([FromBody] RegistroRequest request)
    {
        try
        {
            // Verificar si el usuario ya existe
            var existe = _gestorUsuarios.ObtenerUsuario(request.User);
            if (existe != null)
            {
                return BadRequest("El usuario ya existe.");
            }

            // Crear usuario
            bool exito = _gestorUsuarios.CrearUsuario(request.User, request.Contrasena, "Cliente");
            
            if (exito)
            { 
                return Ok(new { mensaje = "Usuario registrado exitosamente." });
            }
            else
            {
                return StatusCode(500, "Error al crear el usuario.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en registro: {ex.Message}");
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }

    [HttpGet("lista")]
    public IActionResult ListaUsuarios()
    {
        var lista = _gestorUsuarios.ObtenerTodos().Select(u => new
        {
            user = u.User,
            rol = u.ObtenerRol()
        });

        return Ok(lista);
    }
}

public class LoginRequest
{
    public string User { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}

public class RegistroRequest
{
    public string User { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}