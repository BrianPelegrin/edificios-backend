using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.ProjectLayoutEntities;

namespace ProyectoEdificios.Data.EntityConfigurations
{
    public sealed class LayoutUnitConfiguration : IEntityTypeConfiguration<LayoutUnit>
    {
        public void Configure(EntityTypeBuilder<LayoutUnit> builder)
        {
            builder.ToTable("LayoutUnits");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.LayoutBuildingId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.ExternalUnitCode)
                .HasMaxLength(100)
                .IsUnicode(false);                

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.Paid)
                .IsRequired();

            builder.HasOne(x => x.Building)
                .WithMany(x => x.Units)
                .HasForeignKey(x => x.LayoutBuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.LayoutBuildingId);
            builder.HasIndex(x => x.ExternalUnitCode);
            builder.HasIndex(x => new { x.LayoutBuildingId, x.Name });

            // Si quieres evitar códigos externos repetidos globalmente:
            // builder.HasIndex(x => x.ExternalUnitCode).IsUnique();
        }
    }
}
