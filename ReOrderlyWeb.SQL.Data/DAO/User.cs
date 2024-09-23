using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class User
{

    public int userId { get; set; }
    [Required]
    [MaxLength(32)]
    public string name { get; set; }
    [Required]
    [MaxLength(32)]
    public string lastName { get; set; }
    [MaxLength(32)]
    public string? streetName { get; set; }
    public int? houseNumber { get; set; }
    [MaxLength(32)]
    public string? voivodeship { get; set; }
    [MaxLength(32)]
    public string? country { get; set; }
    public int? zipcode { get; set; }
    [Required]
    [MaxLength(32)]
    public string emailAddress { get; set; }
    [MaxLength(32)]
    [Required]
    public string password { get; set; }
    [Required]
    public int phoneNumber { get; set; }
    
    
  
}