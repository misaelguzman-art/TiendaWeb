using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos; // ← Cambiar de Negocio a Datos

public abstract class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(900)]
    public string User { get; set; } = string.Empty;
    
    [Required]
    public string Contrasena { get; set; } = string.Empty;
    
    public int CantidadCompras { get; set; } = 0;
    
    // Relaciones
    public List<Comentarios> Comentarios { get; set; } = new();
    public List<CarritoItem> CarritoItems { get; set; } = new();
    public List<Pedido> Pedidos { get; set; } = new();
    
    public abstract string ObtenerRol();
}