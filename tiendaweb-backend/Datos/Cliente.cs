using tiendaweb_backend.Negocio;  
namespace tiendaweb_backend.Datos;

public class Cliente : Usuario
{ 
    public Carrito CarritoAsociado { get; set; } = new Carrito();
    public Descuento EstrategiaDescuento { get; set; } = new Descuento(0);

    public override string ObtenerRol() => "Cliente";
}