using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderStatus
{
    [Key]
    public int orderStatusId { get; set; }
    [Required]
    [MaxLength(64)]
    public string orderStatusDescription { get; set; }
}