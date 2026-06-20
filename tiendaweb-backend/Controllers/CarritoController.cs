using Microsoft.AspNetCore.Mvc;
using tiendaweb_backend.Negocio;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Contexto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace tiendaweb_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CarritoController : ControllerBase
{
    private readonly GestorUsuarios _gestorUsuarios;
    private readonly GestionProductos _gestionProductos;
    private readonly TiendaDbContext _context;
    private readonly ServicioCompras _servicioCompras;

    public CarritoController(GestorUsuarios gestorUsuarios, GestionProductos gestionProductos, TiendaDbContext context, ServicioCompras servicioCompras)
    {
        _gestorUsuarios = gestorUsuarios;
        _gestionProductos = gestionProductos;
        _context = context;
        _servicioCompras = servicioCompras;
    }

    [HttpPost("{username}/agregar/{productoId}")]
    public IActionResult AgregarAlCarrito(string username, int productoId)
    {
        var usuario = _gestorUsuarios.ObtenerUsuario(username);
        if (usuario == null || !(usuario is Cliente cliente))
            return BadRequest("El usuario no existe o no es un cliente válido.");

        var producto = _gestionProductos.ListaProductos().FirstOrDefault(p => p.Id == productoId);
        if (producto == null)
            return NotFound("Producto no encontrado.");

        // Verificar si el producto ya está en el carrito
        var yaEnCarrito = _context.CarritoItems.Any(c => c.UsuarioId == usuario.Id && c.ProductoId == productoId);
        
        if (!yaEnCarrito)
        {
            var carritoItem = new CarritoItem
            {
                UsuarioId = usuario.Id,
                ProductoId = productoId,
                Cantidad = 1
            };
            _context.CarritoItems.Add(carritoItem);
            _context.SaveChanges();
        }

        return Ok(new { 
            mensaje = yaEnCarrito ? 
                $"El producto {producto.Nombre} ya estaba en el carrito." : 
                $"Producto {producto.Nombre} añadido al carrito.",
            estaEnCarrito = true,
            productoId = productoId
        });
    }

    [HttpDelete("{username}/eliminar/{productoId}")]
    public IActionResult EliminarDelCarrito(string username, int productoId)
    {
        var usuario = _gestorUsuarios.ObtenerUsuario(username);
        if (usuario == null || !(usuario is Cliente cliente))
            return BadRequest("El usuario no existe o no es un cliente válido.");

        var carritoItem = _context.CarritoItems.FirstOrDefault(c => c.UsuarioId == usuario.Id && c.ProductoId == productoId);
        
        if (carritoItem == null)
            return NotFound("El producto no está en el carrito.");

        _context.CarritoItems.Remove(carritoItem);
        _context.SaveChanges();

        return Ok(new { mensaje = $"Producto eliminado del carrito." });
    }

    [HttpGet("{username}/items")]
    public IActionResult ObtenerItems(string username)
    {
        var usuario = _gestorUsuarios.ObtenerUsuario(username);
        if (usuario == null)
            return BadRequest("El usuario no existe.");

        var items = _context.CarritoItems
            .Where(c => c.UsuarioId == usuario.Id)
            .Include(c => c.Producto)
            .AsEnumerable()
            .Select(c => {
                var p = c.Producto;
                var plataforma = (string?)null;
                var duracion = (int?)null;
                
                if (p is ProductoDigital pd)
                {
                    plataforma = pd.Plataforma;
                    duracion = pd.DuracionDias;
                }
                
                return new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    descripcion = p.Descripcion,
                    precio = p.Precio,
                    categoriaId = p.CategoriaId,
                    plataforma = plataforma,
                    duracionDias = duracion,
                    tipo = p.ObtenerTipoProducto()
                };
            })
            .ToList();

        return Ok(items);
    }

    [HttpGet("{username}/total")]
    public IActionResult CalcularTotal(string username)
    {
        var usuario = _gestorUsuarios.ObtenerUsuario(username);
        if (usuario == null)
            return BadRequest("El usuario no existe.");

        var total = _context.CarritoItems
            .Where(c => c.UsuarioId == usuario.Id)
            .Join(_context.Productos,
                c => c.ProductoId,
                p => p.Id,
                (c, p) => p.Precio)
            .Sum();

        // Aplicar descuento si es VIP
        double descuentoPorcentaje = 0;
        if (usuario is ClienteVip)
        {
            descuentoPorcentaje = 20;
            total = total * 0.8; // 20% de descuento
        }

        return Ok(new { 
            total = total,
            descuentoAplicado = descuentoPorcentaje + "%",
            rol = usuario.ObtenerRol(),
            subtotal = total / (1 - descuentoPorcentaje / 100)
        });
    }

    [HttpPost("{username}/comprar")]
    public async Task<IActionResult> Comprar(string username)
    {
        var usuario = _gestorUsuarios.ObtenerUsuario(username);
        if (usuario == null || !(usuario is Cliente cliente))
            return BadRequest("El usuario no existe o no es un cliente válido.");
        
        var carritoItems = _context.CarritoItems.Where(c => c.UsuarioId == usuario.Id).ToList();
        if (carritoItems.Count == 0)
            return BadRequest("El carrito está vacío.");

        var productosIds = carritoItems.Select(c => c.ProductoId).ToList();

        // Usar el ServicioCompras para gestionar la lógica de pedidos, VIP y stock de códigos
        var resultado = await _servicioCompras.RealizarCompra(usuario.Id, productosIds);

        if (!resultado.Exito)
        {
            return BadRequest(resultado.Mensaje);
        }

        // Limpiar carrito si la compra fue exitosa
        _context.CarritoItems.RemoveRange(carritoItems);
        await _context.SaveChangesAsync();

        return Ok(new { 
            mensaje = resultado.Mensaje,
            totalPagado = resultado.TotalPagado,
            fecha = DateTime.Now,
            codigosActivacion = resultado.CodigosActivacion,
            usuarioEsVip = resultado.UsuarioEsVip,
            nuevoRol = resultado.NuevoRol
        });
    }
}