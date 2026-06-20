using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tiendaweb_backend.Contexto;

namespace tiendaweb_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GestionPedidosController : ControllerBase
{
    private readonly TiendaDbContext _context;

    public GestionPedidosController(TiendaDbContext context)
    {
        _context = context;
    }

    [HttpGet("todos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Detalles)
            .OrderByDescending(p => p.FechaPedido)
            .Select(p => new
            {
                p.Id,
                p.FechaPedido,
                p.Total,
                p.Estado,
                Usuario = new { p.Usuario.User },
                CantidadItems = p.Detalles.Count
            })
            .ToListAsync();

        return Ok(pedidos);
    }
}
