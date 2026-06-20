using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos;

public class DetallePedido
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;
    
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    
    public int Cantidad { get; set; }
    public double PrecioUnitario { get; set; }
}