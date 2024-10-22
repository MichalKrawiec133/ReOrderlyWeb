namespace ReOrderlyWeb.ViewModels;

public class OrderSubscriptionViewModel
{
    public int orderSubscriptionId { get; set; }
    public int idUser { get; set; }
    public UserViewModel User { get; set; }
    public int intervalDays { get; set; }
    public DateOnly orderDate { get; set; }
    
    // Kolekcja produktów
    public List<OrderSubscriptionProductViewModel> Products { get; set; }
}

