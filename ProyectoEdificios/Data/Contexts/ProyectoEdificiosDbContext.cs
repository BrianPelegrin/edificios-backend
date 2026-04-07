using Microsoft.EntityFrameworkCore;
using ProyectoEdificios.Models.Entities.Users;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Data.Contexts;

public partial class ProyectoEdificiosDbContext : DbContext
{
    public ProyectoEdificiosDbContext()
    {
    }

    public ProyectoEdificiosDbContext(DbContextOptions<ProyectoEdificiosDbContext> options)
        : base(options)
    {

    }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectLayout> ProjectLayouts => Set<ProjectLayout>();
    public DbSet<LayoutBuilding> LayoutBuildings => Set<LayoutBuilding>();
    public DbSet<LayoutUnit> LayoutUnits => Set<LayoutUnit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProyectoEdificiosDbContext).Assembly);
    }

}
