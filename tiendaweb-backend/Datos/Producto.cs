using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos;

public abstract class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Nombre { get; set; } = string.Empty;
    
    public string? Descripcion { get; set; }
    public double Precio { get; set; }
    public int CategoriaId { get; set; }
    
    // Relaciones
    [System.Text.Json.Serialization.JsonIgnore]
    public Categoria? Categoria { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public List<Comentarios>? Comentarios { get; set; } = new();
    
    [System.Text.Json.Serialization.JsonIgnore]
    public List<CarritoItem>? CarritoItems { get; set; } = new();
    
    public abstract string ObtenerTipoProducto();
}