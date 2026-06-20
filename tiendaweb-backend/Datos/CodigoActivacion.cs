using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos;

public class CodigoActivacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int ProductoId { get; set; }
    public ProductoDigital Producto { get; set; } = null!;
    
    [Required]
    public string Codigo { get; set; } = string.Empty;
    
    public bool EstaUsado { get; set; } = false;
    public int? PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
    public DateTime? FechaAsignacion { get; set; }
}