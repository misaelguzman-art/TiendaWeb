using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos;

public class Pedido
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    
    public DateTime FechaPedido { get; set; } = DateTime.Now;
    public double Total { get; set; }
    public string Estado { get; set; } = "Pendiente";
    
    public List<DetallePedido> Detalles { get; set; } = new();
    public List<CodigoActivacion> CodigosActivacion { get; set; } = new();
}