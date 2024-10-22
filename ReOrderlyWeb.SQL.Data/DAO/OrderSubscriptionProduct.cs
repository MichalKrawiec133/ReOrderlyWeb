using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderSubscriptionProduct
{
    [Key]
    public int orderSubscriptionProductId { get; set; }
    
    [Required]
    public int orderSubscriptionId { get; set; }
    
    public virtual OrderSubscription OrderSubscription { get; set; }

    [Required]
    public int productId { get; set; }
    
    public virtual Products Product { get; set; }

    [Required]
    public int productQuantity { get; set; }
}
