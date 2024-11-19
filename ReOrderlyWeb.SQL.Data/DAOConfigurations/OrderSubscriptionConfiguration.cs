using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class OrderSubscriptionConfiguration : IEntityTypeConfiguration<OrderSubscription>
{
    public void Configure(EntityTypeBuilder<OrderSubscription> builder)
    {
        // Primary key
        builder.HasKey(os => os.orderSubscriptionId);

        // Foreign key relationship with User
        builder.HasOne(os => os.User)
            .WithMany(u => u.OrderSubscriptions)
            .HasForeignKey(os => os.idUser)
            .IsRequired();

        // One-to-many relationship with OrderSubscriptionProducts
        builder.HasMany(os => os.orderSubscriptionProducts)
            .WithOne(osp => osp.OrderSubscription)
            .HasForeignKey(osp => osp.orderSubscriptionId)
            .IsRequired();

        builder.Property(os => os.intervalDays).IsRequired();
        builder.Property(os => os.orderDate).IsRequired();

        builder.ToTable("OrderSubscription");
    }
}

