using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;
using Proyecto.Models.ViewModels;

namespace Proyecto.Datos.DataContext;

public partial class TrabajadoresPruebaContext : DbContext
{
    public TrabajadoresPruebaContext()
    {
    }

    public TrabajadoresPruebaContext(DbContextOptions<TrabajadoresPruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Distrito> Distritos { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Trabajadore> Trabajadores { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aca configuramops TrabajadorVM como una entidad sin llave (Keyless) para permitir 
        // el mapeo directo de los resultados del procedimiento almacenado 'sp_ListarTrabajadores'.
        // Esto evita que EF Core intente rastrearlo como una tabla física de la base de datos.
        modelBuilder.Entity<TrabajadorVM>().HasNoKey();

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433D8D801367");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.IdDistrito).HasName("PK__Distrito__DE8EED59232B529B");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Distritos)
                .HasForeignKey(d => d.IdProvincia)
                .HasConstraintName("FK__Distritos__IdPro__3C69FB99");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProvincia).HasName("PK__Provinci__EED744557C735F89");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Provincia__IdDep__398D8EEE");
        });

        modelBuilder.Entity<Trabajadore>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__Trabajad__6FAFBCF0AA4A9BC9");

            entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento }, "UQ_Documento").IsUnique();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoRuta)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("trabajador.webp");
            entity.Property(e => e.Direccion)
               .HasMaxLength(255)
               .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDistritoNavigation).WithMany(p => p.Trabajadores)
                .HasForeignKey(d => d.IdDistrito)
                .HasConstraintName("FK_Trabajador_Distrito");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
