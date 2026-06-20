using tiendaweb_backend.Contexto;
using tiendaweb_backend.Datos;
using Microsoft.EntityFrameworkCore;

namespace tiendaweb_backend.Negocio;

public class GestorUsuarios
{
    private readonly TiendaDbContext _context;

    public GestorUsuarios(TiendaDbContext context)
    {
        _context = context;
    }

    public Usuario? Autenticar(string user, string pass)
    {
        return _context.Usuarios
            .FirstOrDefault(u => u.User == user && u.Contrasena == pass);
    }

    public bool CrearUsuario(string user, string pass, string rol)
    {
        try
        {
            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.User == user))
                return false;

            Usuario nuevoUsuario;

            // Crear el usuario segun el rol
            switch (rol)
            {
                case "Admin":
                    nuevoUsuario = new Admin { User = user, Contrasena = pass };
                    break;
                case "ClienteVip":
                    nuevoUsuario = new ClienteVip { User = user, Contrasena = pass };
                    break;
                case "Cliente":
                    nuevoUsuario = new Cliente { User = user, Contrasena = pass };
                    break;
                default:
                    return false;
            }

            _context.Usuarios.Add(nuevoUsuario);
            int resultado = _context.SaveChanges();
             
            return resultado > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear usuario: {ex.Message}");
            return false;
        }
    }

    public bool EliminarUsuario(string user)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.User == user);
        if (usuario == null) return false;

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();
        return true;
    }

    public Usuario? ObtenerUsuario(string user)
    {
        return _context.Usuarios.FirstOrDefault(u => u.User == user);
    }

    public List<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }

    public async Task<bool> AscenderAVIP(int usuarioId)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Usuarios SET Discriminator = 'ClienteVip' WHERE Id = {0}",
                usuarioId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}