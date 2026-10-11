using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;

public class BD_Context : DbContext {
    public BD_Context(DbContextOptions<BD_Context> options) : base(options) {}

    public DbSet<Usuario> Salas { get; set; }
    public DbSet<Cine> Cines { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Butaca> Butacas { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Rol> Roles { get; set; }
}