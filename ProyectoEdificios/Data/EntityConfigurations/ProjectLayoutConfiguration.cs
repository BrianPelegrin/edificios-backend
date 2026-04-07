using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Data.EntityConfigurations
{
    public sealed class ProjectLayoutConfiguration : IEntityTypeConfiguration<ProjectLayout>
    {
        public void Configure(EntityTypeBuilder<ProjectLayout> builder)
        {
            builder.ToTable("ProjectLayouts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProjectId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.GridSize)
                .IsRequired();

            builder.HasIndex(x => x.ProjectId)
                .IsUnique();

            builder.HasOne(x => x.Project)
                .WithOne(x => x.Layout)
                .HasForeignKey<ProjectLayout>(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Buildings)
                .WithOne(x => x.ProjectLayout)
                .HasForeignKey(x => x.ProjectLayoutId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
