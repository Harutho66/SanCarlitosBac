using Microsoft.EntityFrameworkCore;

using SanCarlitos.Models;
using System;
using System.Collections.Generic;

namespace SanCarlitos.Data;

public partial class VeterinariaContext : DbContext
{
    public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consulta> Consulta { get; set; }

    public virtual DbSet<Mascota> Mascota { get; set; }

    public virtual DbSet<Propietario> Propietarios { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Veterinario> Veterinarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Consulta__3214EC07D89C4F89");

            entity.HasIndex(e => e.MascotaId, "IX_Consulta_MascotaId");

            entity.HasIndex(e => e.VeterinarioId, "IX_Consulta_VeterinarioId");

            entity.Property(e => e.Costo).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Diagnostico).HasMaxLength(500);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Motivo).HasMaxLength(250);

            entity.HasOne(d => d.Mascota).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.MascotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Mascota");

            entity.HasOne(d => d.Veterinario).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.VeterinarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consulta_Veterinario");
        });

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mascota__3214EC075D36FF9A");

            entity.HasIndex(e => e.PropietarioId, "IX_Mascota_PropietarioId");

            entity.Property(e => e.Especie).HasMaxLength(50);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Raza).HasMaxLength(100);
            entity.Property(e => e.Sexo).HasMaxLength(10);

            entity.HasOne(d => d.Propietario).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.PropietarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascota_Propietario");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Propieta__3214EC07C95AD29D");

            entity.ToTable("Propietario");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tratamie__3214EC07970DFBD3");

            entity.ToTable("Tratamiento");

            entity.HasIndex(e => e.ConsultaId, "IX_Tratamiento_ConsultaId");

            entity.Property(e => e.Costo).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Medicamento).HasMaxLength(150);

            entity.HasOne(d => d.Consulta).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.ConsultaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tratamiento_Consulta");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UX_Usuario_Correo")
                .IsUnique()
                .HasFilter("([Eliminado]=(0))");

            entity.HasIndex(e => e.NombreUsuario, "UX_Usuario_NombreUsuario")
                .IsUnique()
                .HasFilter("([Eliminado]=(0))");

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Usuario_Activo");
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())", "DF_Usuario_FechaRegistro");
            entity.Property(e => e.NombreUsuario).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Rol)
                .HasMaxLength(30)
                .HasDefaultValue("Usuario", "DF_Usuario_Rol");
        });

        modelBuilder.Entity<Veterinario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Veterina__3214EC0779E6DAF3");

            entity.ToTable("Veterinario");

            entity.HasIndex(e => e.MatriculaProfesional, "UQ_Veterinario_Matricula").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.MatriculaProfesional).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
