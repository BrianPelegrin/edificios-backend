using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.Settings;

namespace ProyectoEdificios.Data.EntityConfigurations;

public sealed class UnitColorSettingConfiguration : IEntityTypeConfiguration<UnitColorSetting>
{
    public void Configure(EntityTypeBuilder<UnitColorSetting> builder)
    {
        builder.ToTable("UnitColorSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Estado).HasMaxLength(100).IsRequired();
        builder.Property(x => x.EstadoKey).HasMaxLength(100).IsUnicode(false).IsRequired();
        builder.Property(x => x.ColorCss).HasMaxLength(7).IsUnicode(false).IsRequired();
        builder.HasIndex(x => x.EstadoKey).IsUnique();
    }
}
