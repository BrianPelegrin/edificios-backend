using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoEdificios.Data.Models;

public partial class ProyectoEdificiosDbContext : DbContext
{
    public ProyectoEdificiosDbContext()
    {
    }

    public ProyectoEdificiosDbContext(DbContextOptions<ProyectoEdificiosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Edificio> Edificios { get; set; }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Apartamentos> Apartamentos { get; set; }


    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BRIAN-TUF-F15\\BRIANPELEGRIN;Database=ProyectoEdificiosDB;Trusted_Connection=True;TrustServerCertificate=True;");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Edificio>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__edificio__72E12F1AA1E2E413");

            entity.ToTable("edificios");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Columns).HasColumnName("columns");
            entity.Property(e => e.CubeScale).HasColumnName("cubeScale");
            entity.Property(e => e.FootprintScale).HasColumnName("footprintScale");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(50)
                .HasColumnName("projectID");
            entity.Property(e => e.Rotation).HasColumnName("rotation");
            entity.Property(e => e.Rowspercolumn).HasColumnName("rowspercolumn");
            entity.Property(e => e.TotalUnits).HasColumnName("totalUnits");
            entity.Property(e => e.UnitKeys).HasColumnName("unitKeys");
            entity.Property(e => e.Units).HasColumnName("units");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
            entity.Property(e => e.Z).HasColumnName("z");

            entity.HasOne(d => d.Project).WithMany(p => p.Edificios)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Edificios_Project");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__project__3213E83F77DE65FA");

            entity.ToTable("project");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Municipio)
                .HasMaxLength(100)
                .HasColumnName("municipio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Provincia)
                .HasMaxLength(100)
                .HasColumnName("provincia");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FE77DD8CD");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164465182F2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(255)
                .HasColumnName("clave");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
        });

        modelBuilder.Entity<Apartamentos>().ToTable("Apartamento");


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
