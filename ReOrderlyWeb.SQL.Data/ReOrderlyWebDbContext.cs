using Microsoft.EntityFrameworkCore;

using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data;

public class ReOrderlyWebDbContext : DbContext
{
    public ReOrderlyWebDbContext(DbContextOptions<ReOrderlyWebDbContext> options) : base(options) {}
    
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<OrderItems> OrderItems { get; set; }
    public virtual DbSet<OrderStatus> OrderStatus { get; set; }
    public virtual DbSet<OrderSubscription> OrderSubscription { get; set; }
    public virtual DbSet<Products> Products { get; set; }
    public virtual DbSet<User> User { get; set; }
    
}