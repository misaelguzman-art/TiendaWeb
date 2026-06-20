using System.Linq;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Contexto; 

namespace tiendaweb_backend.Negocio;

public class GestionProductos
{
    private readonly TiendaDbContext _context;

    //nota para mi mismo , esto inyecta el contexto de la base de datos para que pueda acceder a las tablas y realizar operaciones CRUD
    public GestionProductos(TiendaDbContext context)
    {
        _context = context;
    }


    public List<Producto> ListaProductos()
    { 
        return _context.Productos.ToList();
    }

    public List<Producto> ListarPorCategoria(int categoriaId)
    { 
        return _context.Productos.Where(p => p.CategoriaId == categoriaId).ToList();
    }

    public List<Comentarios> ListaComentariosPorProducto(int productoId)
    { 
         return _context.Comentarios.Where(c => c.ProductoId == productoId).ToList();
    }

    public void crearProducto(ProductoDigital producto)
    {
        // Asignar una categoría por defecto si no viene del frontend
        if (producto.CategoriaId == 0)
        {
            var categoriaDefecto = _context.Categorias.FirstOrDefault();
            if (categoriaDefecto != null)
            {
                producto.CategoriaId = categoriaDefecto.Id;
            }
            else
            {
                var nuevaCategoria = new Categoria { Nombre = "General" };
                _context.Categorias.Add(nuevaCategoria);
                _context.SaveChanges();
                producto.CategoriaId = nuevaCategoria.Id;
            }
        }

        _context.Productos.Add(producto);
        _context.SaveChanges();

        // Crear el código de activación asociado si se proporcionó uno
        if (!string.IsNullOrEmpty(producto.CodigoDescarga))
        {
            var codigoActivacion = new CodigoActivacion 
            {
                ProductoId = producto.Id,
                Codigo = producto.CodigoDescarga,
                EstaUsado = false,
                FechaAsignacion = DateTime.Now
            };
            _context.CodigosActivacion.Add(codigoActivacion);
            _context.SaveChanges();
        }
    }

    public void ActualizarProducto(int id, ProductoDigital productoActualizado)
    {
        var producto = _context.Productos.OfType<ProductoDigital>().FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            producto.Nombre = productoActualizado.Nombre;
            producto.Descripcion = productoActualizado.Descripcion;
            producto.Precio = productoActualizado.Precio;
            producto.Plataforma = productoActualizado.Plataforma;
            producto.DuracionDias = productoActualizado.DuracionDias;
            
            if (productoActualizado.CategoriaId > 0)
            {
                producto.CategoriaId = productoActualizado.CategoriaId;
            }

            // Actualizar o agregar código de activación
            if (!string.IsNullOrEmpty(productoActualizado.CodigoDescarga))
            {
                producto.CodigoDescarga = productoActualizado.CodigoDescarga;
                var codigoActivacion = new CodigoActivacion 
                {
                    ProductoId = producto.Id,
                    Codigo = productoActualizado.CodigoDescarga,
                    EstaUsado = false,
                    FechaAsignacion = DateTime.Now
                };
                _context.CodigosActivacion.Add(codigoActivacion);
            }

            _context.SaveChanges();
        }
    }

   public bool EliminarProducto(int id)
    { 
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
        
        if (producto != null)
        { 
            _context.Productos.Remove(producto);
            _context.SaveChanges();
            return true;
        }
        return false;
    }


}