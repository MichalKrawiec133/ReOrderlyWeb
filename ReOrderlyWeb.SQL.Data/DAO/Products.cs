using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Products
{
    [Key]
    public int productId { get; set; }
    [Required]
    [MaxLength(32)]
    public string productName { get; set; }
    [Required]
    public double productPrice { get; set; }
    [Required]
    public int productQuantity { get; set; }
    public string imagePath { get; set; }
    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    
    public virtual ICollection<OrderSubscriptionProduct> OrderSubscriptionProducts { get; set; } = new List<OrderSubscriptionProduct>();
}
    
