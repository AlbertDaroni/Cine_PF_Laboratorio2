using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;

namespace Proyecto_Final.Data;

public class BD_Context : DbContext
{
    public BD_Context(DbContextOptions<BD_Context> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Cine> Cines { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Butaca> Butacas { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Funcion> Funciones { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<DetalleReserva> DetallesReservas { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Director> Directores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pelicula>().ToTable("peliculas");
        modelBuilder.Entity<Director>().ToTable("directores");
        modelBuilder.Entity<Genero>().ToTable("generos");
        modelBuilder.Entity<Rol>().ToTable("roles");

        modelBuilder.Entity<Pelicula>()
            .HasMany(p => p.Generos)
            .WithMany(g => g.Peliculas)
            .UsingEntity<Dictionary<string, object>>(
                "PeliculaGenero",
                j => j.HasOne<Genero>().WithMany().HasForeignKey("genero_id"),
                j => j.HasOne<Pelicula>().WithMany().HasForeignKey("pelicula_id"),
                j => j.ToTable("pelicula_genero")
            );

        modelBuilder.Entity<Pelicula>()
            .HasMany(p => p.Directores)
            .WithMany(d => d.Peliculas)
            .UsingEntity<Dictionary<string, object>>(
                "PeliculaDirector",
                j => j.HasOne<Director>().WithMany().HasForeignKey("director_id"),
                j => j.HasOne<Pelicula>().WithMany().HasForeignKey("pelicula_id"),
                j => j.ToTable("pelicula_director")
            );
    }
}