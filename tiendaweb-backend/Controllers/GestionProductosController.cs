using Microsoft.AspNetCore.Mvc;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Negocio;

namespace tiendaweb_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GestionProductosController : ControllerBase
{
    private readonly GestionProductos _gestionProductos;

    public GestionProductosController(GestionProductos gestionProductos)
    {
        _gestionProductos = gestionProductos;
    }

    [HttpGet("lista-productos")]
    public IEnumerable<Producto> ListaProductos()
    {
        return _gestionProductos.ListaProductos();
    }

    [HttpGet("categoria/{categoriaId}")]
    public IEnumerable<Producto> ListaPorCategoria(int categoriaId)
    {
        return _gestionProductos.ListarPorCategoria(categoriaId);
    }

    [HttpPost("crear-producto")]
    public void CrearProducto(ProductoDigital producto)
    {
        _gestionProductos.crearProducto(producto);
    }

    [HttpPut("actualizar-producto/{id}")]
    public IActionResult ActualizarProducto(int id, [FromBody] ProductoDigital producto)
    {
        _gestionProductos.ActualizarProducto(id, producto);
        return Ok(new { mensaje = "Producto actualizado con éxito." });
    }

    [HttpDelete("eliminar-producto/{id}")]
    public IActionResult EliminarProducto(int id)
    {
        try
        {
            bool eliminado = _gestionProductos.EliminarProducto(id);
            if (eliminado)
                return Ok(new { mensaje = "Producto eliminado con éxito." });
            else
                return NotFound(new { mensaje = "Producto no encontrado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }


     [HttpGet("comentarios/{productoId}")]
    public IEnumerable<Comentarios> ListaComentariosPorProducto(int productoId)
    {
        return _gestionProductos.ListaComentariosPorProducto(productoId);
    }


}