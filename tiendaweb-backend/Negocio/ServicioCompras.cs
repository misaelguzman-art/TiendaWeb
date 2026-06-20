using Microsoft.EntityFrameworkCore;
using tiendaweb_backend.Contexto;
using tiendaweb_backend.Datos;

namespace tiendaweb_backend.Negocio;

public class ServicioCompras
{
    private readonly TiendaDbContext _context;

    public ServicioCompras(TiendaDbContext context)
    {
        _context = context;
    }

    public class ResultadoCompra
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public double TotalPagado { get; set; }
        public List<object> CodigosActivacion { get; set; } = new();
        public bool UsuarioEsVip { get; set; }
        public string NuevoRol { get; set; } = string.Empty;
    }

    public async Task<ResultadoCompra> RealizarCompra(int usuarioId, List<int> productosIds)
    {
        var resultado = new ResultadoCompra { Exito = false };
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null) 
            {
                resultado.Mensaje = "Usuario no encontrado.";
                return resultado;
            }

            var productos = await _context.Productos
                .Where(p => productosIds.Contains(p.Id))
                .ToListAsync();

            if (!productos.Any())
            {
                resultado.Mensaje = "No hay productos válidos.";
                return resultado;
            }

            double totalCalculado = productos.Sum(p => p.Precio);

            // Aplicar descuento VIP del 20%
            if (usuario is ClienteVip)
            {
                totalCalculado = totalCalculado * 0.8;
            }

            // Crear pedido
            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Total = totalCalculado,
                Estado = "Pagado"
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var codigosAsignados = new List<object>();

            // Agregar detalles
            foreach (var producto in productos)
            {
                _context.DetallesPedido.Add(new DetallePedido
                {
                    PedidoId = pedido.Id,
                    ProductoId = producto.Id,
                    Cantidad = 1,
                    PrecioUnitario = producto.Precio
                });

                producto.CategoriaId = -1; // Marcar como vendido

                string codigoAsignado = "";
                if (producto is ProductoDigital pd)
                {
                    var codigo = await _context.CodigosActivacion
                        .FirstOrDefaultAsync(c => c.ProductoId == producto.Id && !c.EstaUsado);

                    if (codigo != null)
                    {
                        codigo.EstaUsado = true;
                        codigo.PedidoId = pedido.Id;
                        codigo.FechaAsignacion = DateTime.Now;
                        codigoAsignado = codigo.Codigo;
                    }
                    else
                    {
                        codigoAsignado = pd.CodigoDescarga ?? $"CODE-{DateTime.Now.Ticks}";
                    }
                    
                    codigosAsignados.Add(new { nombreProducto = producto.Nombre, codigo = codigoAsignado, precio = producto.Precio });
                }
            }

            // Actualizar contador de compras
            usuario.CantidadCompras += productos.Count;

            // Verificar ascenso a VIP
            if (usuario.CantidadCompras >= 3 && usuario is Cliente && !(usuario is ClienteVip))
            {
                // Ascender a VIP
                var nuevoVip = new ClienteVip
                {
                    Id = usuario.Id,
                    User = usuario.User,
                    Contrasena = usuario.Contrasena,
                    CantidadCompras = usuario.CantidadCompras
                };
                
                // En EF, para cambiar el tipo (TPH), usamos ExecuteSqlRawAsync o borramos e insertamos.
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Usuarios SET Discriminator = 'ClienteVip' WHERE Id = {0}",
                    usuarioId);
                
                resultado.NuevoRol = "ClienteVip";
                resultado.UsuarioEsVip = true;
            }
            else
            {
                resultado.NuevoRol = usuario.ObtenerRol();
                resultado.UsuarioEsVip = usuario is ClienteVip;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            resultado.Exito = true;
            resultado.Mensaje = "Compra realizada con éxito";
            resultado.TotalPagado = totalCalculado;
            resultado.CodigosActivacion = codigosAsignados;

            return resultado;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            resultado.Mensaje = "Error al procesar compra: " + ex.Message;
            return resultado;
        }
    }
}