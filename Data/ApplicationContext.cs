using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Models;

namespace Pruba_tienda_api.Data;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Perfile> Perfiles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosPerfile> UsuariosPerfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-B1EMHD1; Database=db_tienda; Trusted_Connection=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC279B56BBA0");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detalles__3214EC27D82469E0");

            entity.ToTable("Detalles_Pedido");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PedidoId).HasColumnName("Pedido_ID");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("Producto_ID");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK__Detalles___Pedid__37A5467C");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__Detalles___Produ__38996AB5");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pedidos__3214EC274A2BE597");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Pedidos__Cliente__34C8D9D1");
        });

        modelBuilder.Entity<Perfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Perfiles__3214EC274118D917");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NombrePerfil)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Perfil");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC2789229B5C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripción).HasColumnType("text");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasMany(d => d.Categoria).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Categ__31EC6D26"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Productos__Produ__30F848ED"),
                    j =>
                    {
                        j.HasKey("ProductoId", "CategoriaId").HasName("PK__Producto__23898F2D80CC2226");
                        j.ToTable("Productos_Categorias");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("Producto_ID");
                        j.IndexerProperty<int>("CategoriaId").HasColumnName("Categoria_ID");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC277BB88CFE");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534BC595288").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsuariosPerfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC270577EE2A");

            entity.ToTable("Usuarios_Perfiles");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PerfilId).HasColumnName("Perfil_ID");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Perfil).WithMany(p => p.UsuariosPerfiles)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("FK__Usuarios___Perfi__2A4B4B5E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuariosPerfiles)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Usuarios___Usuar__29572725");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
