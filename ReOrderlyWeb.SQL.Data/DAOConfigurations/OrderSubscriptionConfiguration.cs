using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class OrderSubscriptionConfiguration : IEntityTypeConfiguration<OrderSubscription>
{
    public void Configure(EntityTypeBuilder<OrderSubscription> builder)
    {
        builder.HasKey(os => os.orderSubscriptionId);
        
        /*
        builder.HasOne(os => os.User)
            .WithMany()                   
            .HasForeignKey(os => os.idUser)
            .IsRequired();

        builder.HasOne(os => os.Products)
            .WithMany()                     
            .HasForeignKey(os => os.idProduct)
            .IsRequired();
            */

        
        builder.Property(os => os.idUser).IsRequired();
        builder.Property(os => os.idProduct).IsRequired();
        builder.Property(os => os.productQuantity).IsRequired();
        builder.Property(os => os.intervalDays).IsRequired();
        builder.Property(os => os.orderDate).IsRequired();

        builder.ToTable("OrderSubscription");
    }
}
