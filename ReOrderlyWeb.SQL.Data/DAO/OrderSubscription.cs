using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderSubscription
{
    [Key]
    public int orderSubscriptionId { get; set; }
    
    [Required]
    public int idUser { get; set; }
    
    public virtual User User { get; set; }
    
    [Required]
    public int intervalDays { get; set; }
    
    [Required]
    public DateOnly orderDate { get; set; }
    
    public virtual ICollection<OrderSubscriptionProduct> orderSubscriptionProducts { get; set; } = new List<OrderSubscriptionProduct>();
}
