using Microsoft.EntityFrameworkCore;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.Services;

public class SubscriptionOrderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SubscriptionOrderService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); //tu mozna do testow zmienic na fromseconds np 5 i sie od razu wykona. 

    public SubscriptionOrderService(IServiceScopeFactory scopeFactory, ILogger<SubscriptionOrderService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Sprawdzanie subskrypcji...");

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ReOrderlyWebDbContext>();
                await ProcessSubscriptions(context);
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ProcessSubscriptions(ReOrderlyWebDbContext context)
    {
        var today = DateTime.Today;
        var subscriptionsToProcess = await context.OrderSubscription
            .Include(s => s.orderSubscriptionProducts) 
            .Where(s => today >= s.orderDate.ToDateTime(TimeOnly.MinValue).AddDays(s.intervalDays))
            .ToListAsync(); 
        //include zapewnia pobranie powiązanych danych do ordersubscription, bez tego nie pobiera products.
        foreach (var subscription in subscriptionsToProcess)
        {
            await CreateOrderFromSubscription(context, subscription);
        }

        await context.SaveChangesAsync();
    }


    private async Task CreateOrderFromSubscription(ReOrderlyWebDbContext context, OrderSubscription subscription)
    {
        var newOrder = new Order
        {
            idUser = subscription.idUser,
            idOrderStatus = 1, 
            orderDate = DateTime.Now
        };

        context.Order.Add(newOrder);
        await context.SaveChangesAsync(); 

        foreach (var subscriptionProduct in subscription.orderSubscriptionProducts)
        {
            var product = await context.Products.FindAsync(subscriptionProduct.productId);

            if (product != null)
            {
                double itemTotalPrice = product.productPrice * subscriptionProduct.productQuantity; 
                
                var orderItem = new OrderItems
                {
                    idOrder = newOrder.orderId,
                    idProduct = product.productId,
                    orderItemQuantity = subscriptionProduct.productQuantity,
                    orderPrice = itemTotalPrice 
                };

                context.OrderItems.Add(orderItem);
                
            }
            
        }

        subscription.orderDate = DateOnly.FromDateTime(DateTime.Now);

        await context.SaveChangesAsync(); 
        
    }



}
