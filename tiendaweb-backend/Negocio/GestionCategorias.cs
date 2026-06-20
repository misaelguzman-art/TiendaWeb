using System.Collections.Generic;
using tiendaweb_backend.Datos;

namespace tiendaweb_backend.Negocio;

public class GestionCategorias
{
    public List<Categoria> ListaCategorias()
    {
        var result = new List<Categoria>
        {
            new() {Id = 1, Nombre = "Streaming"}, // ← SIN DESCRIPCION
            new() {Id = 2, Nombre = "Software"}   // ← SIN DESCRIPCION
        };

        return result;
    }
}