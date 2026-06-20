using Microsoft.AspNetCore.Mvc;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Negocio;

namespace tiendaweb_backend.Controllers;


[ApiController]
[Route("[controller]")]

public class GestionComentariosController : ControllerBase
{
    private readonly GestionComentarios gestionComentarios;

    public GestionComentariosController(GestionComentarios _gestionComentarios)
    {
        gestionComentarios = _gestionComentarios;
    }

    
    [HttpGet("comentarios-por-producto/{productoId}")]
    public IEnumerable<Comentarios> ListaComentariosPorProducto(int productoId)
    {
        return gestionComentarios.ListaComentarios().Where(c => c.ProductoId == productoId).ToList();
    }

    [HttpGet("comentarios-por-usuario/{usuarioId}")]
    public IEnumerable<Comentarios> ListaComentariosPorUsuario(int usuarioId)
    {
        return gestionComentarios.ListaComentarios().Where(c => c.UsuarioId == usuarioId).ToList();
     }

    [HttpDelete("eliminar-comentario/{id}")]
    public IActionResult EliminarComentario(int id)
    {
        var comentario = gestionComentarios.ListaComentarios().FirstOrDefault(c => c.Id == id);
        if (comentario != null)
        {
            gestionComentarios.EliminarComentario(id);
            return Ok(new { mensaje = "Comentario eliminado con éxito." });
        }
        else
            return NotFound("Comentario no encontrado.");
     }
     

    [HttpPut("editar-comentario/{id}")]
    public IActionResult EditarComentario(int id, [FromBody] string nuevoTexto)
    {
        var comentario = gestionComentarios.ListaComentarios().FirstOrDefault(c => c.Id == id);
        if (comentario != null)
        {
            comentario.Texto = nuevoTexto;
            return Ok(new { mensaje = "Comentario editado con éxito." });
        }
        else
            return NotFound("Comentario no encontrado.");
    }

     [HttpGet("MostrarTodos")]
    public IEnumerable<Comentarios> MostrarTodos()
    {        
        
        return gestionComentarios.MostrarTodos();
     }

    [HttpPost("AgregarComentario")]
    public IActionResult AgregarComentario([FromBody] Comentarios nuevoComentario)
        {
            if (nuevoComentario != null)
            {
                gestionComentarios.AgregarComentario(nuevoComentario);
                return Ok(new { mensaje = "Comentario agregado con éxito." });
            }
            else
            {
                return BadRequest("Datos del comentario no válidos.");
            }
    }
    
}
