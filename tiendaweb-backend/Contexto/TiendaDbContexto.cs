using Microsoft.EntityFrameworkCore;
using tiendaweb_backend.Datos;
using tiendaweb_backend.Negocio;

namespace tiendaweb_backend.Contexto;

public class TiendaDbContext : DbContext
{
    public TiendaDbContext(DbContextOptions<TiendaDbContext> options) : base(options) { }

    // SOLO ESTOS DbSets
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Comentarios> Comentarios { get; set; }

    public DbSet<CarritoItem> CarritoItems { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }                
    public DbSet<DetallePedido> DetallesPedido { get; set; }   
    public DbSet<CodigoActivacion> CodigosActivacion { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.Ignore<Carrito>();

        modelBuilder.Ignore<Descuento>();

        // TPH Usuarios
        modelBuilder.Entity<Usuario>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Admin>("Admin")
            .HasValue<Cliente>("Cliente")
            .HasValue<ClienteVip>("ClienteVip");

        // TPH Productos
        modelBuilder.Entity<Producto>()
            .HasDiscriminator<string>("TipoProducto")
            .HasValue<ProductoDigital>("Digital");

        // Relaciones Comentarios
        modelBuilder.Entity<Comentarios>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.UsuarioId);

        modelBuilder.Entity<Comentarios>()
            .HasOne(c => c.Producto)
            .WithMany(p => p.Comentarios)
            .HasForeignKey(c => c.ProductoId);

        base.OnModelCreating(modelBuilder);
    }
}