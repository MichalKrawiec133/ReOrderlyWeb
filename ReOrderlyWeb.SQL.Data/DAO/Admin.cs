using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Admin
{
    [Key]
    public int adminId { get; set; } 

    [Required]
    public int userId { get; set; } 

    [Required]
    public virtual User User { get; set; }
}