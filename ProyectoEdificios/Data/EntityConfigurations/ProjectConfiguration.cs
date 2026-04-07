using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Data.EntityConfigurations
{
    public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Address)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.Province)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Municipality)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.PlanImageUrl);

            builder.HasOne(x => x.Layout)
                .WithOne(x => x.Project)
                .HasForeignKey<ProjectLayout>(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => new { x.Province, x.Municipality });
        }
    }
}
