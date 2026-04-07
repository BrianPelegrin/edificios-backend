using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Data.EntityConfigurations
{
    public sealed class LayoutBuildingConfiguration : IEntityTypeConfiguration<LayoutBuilding>
    {
        public void Configure(EntityTypeBuilder<LayoutBuilding> builder)
        {
            builder.ToTable("LayoutBuildings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.ProjectLayoutId)
                .IsRequired();

            builder.Property(x => x.PositionX).HasColumnType("float");
            builder.Property(x => x.PositionZ).HasColumnType("float");
            builder.Property(x => x.RotationY).HasColumnType("float");

            builder.Property(x => x.Width)
                .IsRequired();

            builder.Property(x => x.Depth)
                .IsRequired();

            builder.Property(x => x.Height)
                .IsRequired();

            builder.HasOne(x => x.ProjectLayout)
                .WithMany(x => x.Buildings)
                .HasForeignKey(x => x.ProjectLayoutId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Units)
                .WithOne(x => x.Building)
                .HasForeignKey(x => x.LayoutBuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ProjectLayoutId);
            builder.HasIndex(x => new { x.ProjectLayoutId, x.Name });
        }
    }
}
