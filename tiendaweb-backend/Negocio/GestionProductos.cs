using tiendaweb_backend.Datos;

namespace tiendaweb_backend.Negocio;

public class GestionProductos
{
    public List<Producto> ListaProductos()
    {
        //en lugar de hacer una lista estatica, se debe llamar a la base de datos
        var result = new List<Producto>
        {
            new() {Id = 1, Nombre = "Mouse", Descripcion = "hardware", Precio = 12.45},
            new() {Id = 2, Nombre = "Monitor", Descripcion = "pantalla", Precio = 100.0},
            new() {Id = 3, Nombre = "Teclado", Descripcion = "mecanico", Precio = 50.5},
            new() {Id = 4, Nombre = "Mousepad", Descripcion = "pad lg", Precio = 12.5},
            new() {Id = 5, Nombre = "Hub", Descripcion = "multiple", Precio = 99.5}
        };

        return result;
    }
}