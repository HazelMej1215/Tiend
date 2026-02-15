using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TiendaApi.Models;

public partial class TiendaDbContext : DbContext
{
    public TiendaDbContext()
    {
    }

    public TiendaDbContext(DbContextOptions<TiendaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<EstadosProducto> EstadosProductos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=tienda_admin;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("id_categoria");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'ACTIVO'")
                .HasColumnType("enum('ACTIVO','INACTIVO')")
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosProducto>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PRIMARY");

            entity.ToTable("estados_producto");

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.IdEstado)
                .ValueGeneratedNever()
                .HasColumnType("tinyint(4)")
                .HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.IdCategoria, "idx_productos_categoria");

            entity.HasIndex(e => e.IdEstado, "idx_productos_estado");

            entity.HasIndex(e => e.Nombre, "idx_productos_nombre");

            entity.HasIndex(e => e.Sku, "sku").IsUnique();

            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.ActualizadoEn)
                .HasColumnType("datetime")
                .HasColumnName("actualizado_en");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("id_categoria");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Sku)
                .HasMaxLength(60)
                .HasColumnName("sku");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_categoria");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_estado");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("creado_en");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'ACTIVO'")
                .HasColumnType("enum('ACTIVO','INACTIVO')")
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'CLIENTE'")
                .HasColumnType("enum('ADMIN','CLIENTE')")
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PRIMARY");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.IdUsuario, "fk_ventas_usuario");

            entity.HasIndex(e => e.Estatus, "idx_ventas_estatus");

            entity.HasIndex(e => e.Fecha, "idx_ventas_fecha");

            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.Estatus)
                .HasDefaultValueSql("'PAGADA'")
                .HasColumnType("enum('PAGADA','CANCELADA')")
                .HasColumnName("estatus");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_ventas_usuario");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");

            entity.ToTable("venta_detalle");

            entity.HasIndex(e => e.IdProducto, "idx_detalle_producto");

            entity.HasIndex(e => e.IdVenta, "idx_detalle_venta");

            entity.Property(e => e.IdDetalle)
                .HasColumnType("int(11)")
                .HasColumnName("id_detalle");
            entity.Property(e => e.Cantidad)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_producto");
            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.Subtotal)
                .HasPrecision(12, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("fk_detalle_venta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
