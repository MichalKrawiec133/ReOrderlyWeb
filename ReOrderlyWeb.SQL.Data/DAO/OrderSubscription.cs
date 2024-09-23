using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderSubscription
{
    public int orderSubscriptionId { get; set; }
    [Required]
    public int idUser { get; set; }
    [Required]
    public int idProduct { get; set; }
    [Required]
    public int intervalDays { get; set; }
    [Required]
    public DateOnly orderDate { get; set; }
    
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    
    public virtual ICollection<Products> Productss { get; set; } = new List<Products>();
}