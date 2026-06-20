using System.Collections.Generic;
using tiendaweb_backend.Datos;

namespace tiendaweb_backend.Negocio;

public class Carrito
{
    private List<Producto> items;

    public Carrito()
    {
        items = new List<Producto>();
    }

    public void AgregarProducto(Producto producto)
    {
        // Verificar si el producto ya está en el carrito
        if (!items.Any(p => p.Id == producto.Id))
        {
            items.Add(producto);
        }
    }

    public void EliminarProducto(Producto producto)
    {
        items.Remove(producto);
    }

    public List<Producto> ObtenerItems()
    {
        return items;
    }

    public double CalcularTotal(Descuento descuento)
    {
        double subtotal = 0;
        foreach (var item in items)
        {
            subtotal += item.Precio;
        }

        double valorDescuento = descuento.CalcularDescuentoBase(subtotal);
        return subtotal - valorDescuento;
    }

    public double Comprar(Descuento descuento)
    {
        double total = CalcularTotal(descuento);
        items.Clear();
        return total;
    }
}
