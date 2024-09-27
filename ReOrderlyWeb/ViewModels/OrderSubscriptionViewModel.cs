namespace ReOrderlyWeb.ViewModels;

public class OrderSubscriptionViewModel
{
    public int orderSubscriptionId { get; set; }

    public int idUser { get; set; }
    
    public int idProduct { get; set; }
    
    public int productQuantity { get; set; }

    public int intervalDays { get; set; }

    public DateOnly orderDate { get; set; }
}