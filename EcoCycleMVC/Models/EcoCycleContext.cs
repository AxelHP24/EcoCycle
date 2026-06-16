using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Models;

public partial class EcoCycleContext : DbContext
{
    public EcoCycleContext()
    {
    }

    public EcoCycleContext(DbContextOptions<EcoCycleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Canje> Canjes { get; set; }

    public virtual DbSet<CentrosRecoleccion> CentrosRecoleccions { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<Materiale> Materiales { get; set; }

    public virtual DbSet<MovimientosPunto> MovimientosPuntos { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Publicacione> Publicaciones { get; set; }

    public virtual DbSet<RecogidasDomicilio> RecogidasDomicilios { get; set; }

    public virtual DbSet<Recompensa> Recompensas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EcoCycle;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Canje>(entity =>
        {
            entity.HasKey(e => e.CanjeId).HasName("PK__Canjes__205E1ABD745723F6");

            entity.Property(e => e.CodigoCupon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaCanje)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Recompensa).WithMany(p => p.Canjes)
                .HasForeignKey(d => d.RecompensaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Canjes_Recompensas");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Canjes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Canjes_Usuarios");
        });

        modelBuilder.Entity<CentrosRecoleccion>(entity =>
        {
            entity.HasKey(e => e.CentroId).HasName("PK__CentrosR__A7CA3F20DAA78FB4");

            entity.ToTable("CentrosRecoleccion");

            entity.Property(e => e.CapacidadActual).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CapacidadMaxima).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Latitud).HasColumnType("decimal(10, 8)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(11, 8)");
            entity.Property(e => e.NombreCentro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.EntregaId).HasName("PK__Entregas__D9AD2303E6764316");

            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaEntrega)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PesoValidado).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Centro).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.CentroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entregas_Centros");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entregas_Publicaciones");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entregas_Usuarios");
        });

        modelBuilder.Entity<Materiale>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F710BC177B");

            entity.HasIndex(e => e.NombreMaterial, "UQ__Material__8A478BCF437C7B15").IsUnique();

            entity.Property(e => e.NombreMaterial)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PuntosPorKg).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<MovimientosPunto>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__Movimien__BF923C2CA4F553A5");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.MovimientosPuntos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimientos_Usuarios");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("PK__Notifica__BCC120245919E208");

            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Leida).HasDefaultValue(false);
            entity.Property(e => e.Mensaje)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");
        });

        modelBuilder.Entity<Publicacione>(entity =>
        {
            entity.HasKey(e => e.PublicacionId).HasName("PK__Publicac__10DF158AE2A3AF83");

            entity.Property(e => e.CantidadKg).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Disponible");
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.Material).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicaciones_Materiales");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Publicaciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publicaciones_Usuarios");
        });

        modelBuilder.Entity<RecogidasDomicilio>(entity =>
        {
            entity.HasKey(e => e.RecogidaId).HasName("PK__Recogida__C54F239EE32239FB");

            entity.ToTable("RecogidasDomicilio");

            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FechaProgramada).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Centro).WithMany(p => p.RecogidasDomicilios)
                .HasForeignKey(d => d.CentroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recogidas_Centros");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RecogidasDomicilios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recogidas_Usuarios");
        });

        modelBuilder.Entity<Recompensa>(entity =>
        {
            entity.HasKey(e => e.RecompensaId).HasName("PK__Recompen__351687DDDCF88213");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B802BBC156");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19C006E4C9").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoPerfil)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Puntos).HasDefaultValue(0);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Ciudadano");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
