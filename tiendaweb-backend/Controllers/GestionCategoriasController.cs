using Microsoft.AspNetCore.Mvc;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Negocio;

namespace tiendaweb_backend.Controllers;


[ApiController]
[Route("[controller]")]

public class GestionCategoriasController : ControllerBase
{
    private GestionCategorias gestionCategorias;

    public GestionCategoriasController()
    {
        gestionCategorias = new GestionCategorias();
    }

    [HttpGet("lista-categorias")]
    public IEnumerable<Categoria> ListaCategorias()
    { 
        return gestionCategorias.ListaCategorias();
    }

    
}
