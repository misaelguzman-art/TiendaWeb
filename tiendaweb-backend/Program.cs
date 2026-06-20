using Microsoft.EntityFrameworkCore;
using tiendaweb_backend.Contexto;
using tiendaweb_backend.Negocio; // <--- AGREGADO para que reconozca tus clases de negocio

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. Configurar Entity Framework  
builder.Services.AddDbContext<TiendaDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// 2. Dependency Injection: 
builder.Services.AddScoped<GestionProductos>();
builder.Services.AddScoped<GestorUsuarios>(); 
builder.Services.AddScoped<GestionCategorias>();  
builder.Services.AddScoped<ServicioCompras>(); 
builder.Services.AddScoped<GestionComentarios>(); 


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")  
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();