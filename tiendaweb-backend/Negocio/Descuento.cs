namespace tiendaweb_backend.Negocio; // ← Cambiado de Datos a Negocio

public class Descuento
{
    public double Porcentaje { get; set; }

    public Descuento(double porcentaje)
    {
        this.Porcentaje = porcentaje;
    }

    public virtual double CalcularDescuentoBase(double subtotal)
    {
        return subtotal * (Porcentaje / 100.0);
    }
}