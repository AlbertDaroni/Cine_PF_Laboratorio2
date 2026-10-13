using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;

namespace Proyecto_Final.Data;

public class BD_Context : DbContext {
    public BD_Context(DbContextOptions<BD_Context> options) : base(options) {}

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cine> Cines { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Butaca> Butacas { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Rol> Roles { get; set; }
}