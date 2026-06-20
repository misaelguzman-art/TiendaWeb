using tiendaweb_backend.Negocio; 

namespace tiendaweb_backend.Datos;

public class ClienteVip : Cliente
{
    public ClienteVip()
    {
        EstrategiaDescuento = new Descuento(20); // 20% descuento
    }

    public override string ObtenerRol() => "ClienteVip";
}