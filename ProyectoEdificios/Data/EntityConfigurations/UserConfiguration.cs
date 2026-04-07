using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoEdificios.Models.Entities.Users;

namespace ProyectoEdificios.Data.Configurations.Auth
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Codigo)
                .HasMaxLength(50);

            builder.Property(x => x.Nombre)
                .HasMaxLength(150);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.Clave)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Salt)
                .HasMaxLength(200);

            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("viewer");

            builder.Property(x => x.RefreshTokenHash)
                .HasMaxLength(500);

            builder.Property(x => x.RefreshTokenExpiresAtUtc);

            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}