namespace tiendaweb_backend.Datos; // ← Cambiar a Datos

public class Admin : Usuario
{
    public override string ObtenerRol() => "Admin";
}