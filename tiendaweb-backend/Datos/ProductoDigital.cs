namespace tiendaweb_backend.Datos;

public class ProductoDigital : Producto
{
    public string? Plataforma { get; set; }
    public int DuracionDias { get; set; }
    public string? CodigoDescarga { get; set; }
    
    public override string ObtenerTipoProducto() => "Producto Digital";
}