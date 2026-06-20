using System.Collections.Generic;
using System; // ← AGREGAR ESTO
using tiendaweb_backend.Datos;

namespace tiendaweb_backend.Negocio;

public class GestionComentarios
{
    private readonly Contexto.TiendaDbContext _context;

    public GestionComentarios(Contexto.TiendaDbContext context)
    {
        _context = context;
    }

    public List<Comentarios> ListaComentarios()
    {
        return _context.Comentarios.ToList();
    }

    public List<Comentarios> MostrarTodos()
    {
        return this.ListaComentarios();
    }

    public List<Comentarios> ListaComentariosPorProducto(int productoId)
    {
        return _context.Comentarios.Where(c => c.ProductoId == productoId).ToList();
    }

    public List<Comentarios> ListaComentariosPorUsuario(int usuarioId)
    {
        return _context.Comentarios.Where(c => c.UsuarioId == usuarioId).ToList();
    }

    public void EliminarComentario(int id)
    {
        var comentario = _context.Comentarios.FirstOrDefault(c => c.Id == id);
        if (comentario != null)
        {
            _context.Comentarios.Remove(comentario);
            _context.SaveChanges();
        }
    }

    public void EditarComentario(int id, string nuevoTexto)
    {
        var comentario = _context.Comentarios.FirstOrDefault(c => c.Id == id);
        if (comentario != null)
        {
            comentario.Texto = nuevoTexto;
            _context.SaveChanges();
        }
    }

    public void AgregarComentario(Comentarios nuevoComentario)
    {
        // Si el usuarioId es 0 (ej. no encontrado en LocalStorage), asignar a un usuario por defecto
        if (nuevoComentario.UsuarioId == 0)
        {
            var admin = _context.Usuarios.FirstOrDefault();
            if (admin != null) nuevoComentario.UsuarioId = admin.Id;
        }

        _context.Comentarios.Add(nuevoComentario);
        _context.SaveChanges();
    }
}