using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tiendaweb_backend.Datos;

public class Comentarios
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Texto { get; set; } = string.Empty;
    
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
    public string? Fecha { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public int Calificacion { get; set; } = 0;
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Usuario? Usuario { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Producto? Producto { get; set; }
}