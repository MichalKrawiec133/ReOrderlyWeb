namespace ReOrderlyWeb.ViewModels;

public class UserViewModel
{
   
    public int userId { get; set; }
    
    public string name { get; set; }
    public string lastName { get; set; }
    
    public string? streetName { get; set; }
    public int? houseNumber { get; set; }
 
    public string? voivodeship { get; set; }

    public string? country { get; set; }
    public int? zipcode { get; set; }


    public string emailAddress { get; set; }

    public string password { get; set; } = "";

    public int phoneNumber { get; set; }
    

}