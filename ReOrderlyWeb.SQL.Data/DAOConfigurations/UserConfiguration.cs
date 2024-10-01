using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.userId);

        builder.Property(u => u.name).IsRequired().HasMaxLength(32);
        builder.Property(u => u.lastName).IsRequired().HasMaxLength(32);
        builder.Property(u => u.streetName).HasMaxLength(32);
        builder.Property(u => u.houseNumber).HasMaxLength(32);
        builder.Property(u => u.voivodeship).HasMaxLength(32);
        builder.Property(u => u.country).HasMaxLength(32);
        builder.Property(u => u.zipcode).HasMaxLength(32);
        builder.Property(u => u.emailAddress).IsRequired().HasMaxLength(32);
        builder.Property(u => u.password).IsRequired().HasMaxLength(32);
        builder.Property(u => u.phoneNumber).IsRequired();

        builder.ToTable("User");
    }
}