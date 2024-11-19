using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.adminId); 

        builder.Property(a => a.userId).IsRequired(); 

        builder.HasOne(a => a.User)
            .WithMany() 
            .HasForeignKey(a => a.userId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.ToTable("Admin"); 
    }
}