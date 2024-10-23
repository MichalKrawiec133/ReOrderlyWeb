using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.ViewModels;
using System.Security.Claims;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.Controllers
{
    
    [ApiController]
    [Authorize]
    public class OrderSubscriptionController : ControllerBase
    {
        private readonly ReOrderlyWebDbContext _context;

        public OrderSubscriptionController(ReOrderlyWebDbContext context)
        {
            _context = context;
        }
            
    // Wyświetlenie wszystkich subskrypcji
    [HttpGet("subscriptions")]
public IActionResult GetCurrentSubscriptions()
{
    var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    if (string.IsNullOrEmpty(email))
    {
        return Unauthorized("No valid user session.");
    }

    var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

    if (user == null)
    {
        return Unauthorized("User not found.");
    }

    var subscriptions = _context.OrderSubscription
        .Where(o => o.idUser == user.userId)
        .Select(o => new OrderSubscriptionViewModel
        {
            orderSubscriptionId = o.orderSubscriptionId,
            User = new UserViewModel
            {
                userId = o.User.userId,
                name = o.User.name,
                lastName = o.User.lastName,
                streetName = o.User.streetName,
                houseNumber = o.User.houseNumber,
                voivodeship = o.User.voivodeship,
                country = o.User.country,
                zipcode = o.User.zipcode,
                emailAddress = o.User.emailAddress,
                phoneNumber = o.User.phoneNumber
            },

            OrderSubscriptionProducts = o.OrderSubscriptionProducts.Select(osp => new OrderSubscriptionProductViewModel
            {
                orderSubscriptionProductId = osp.orderSubscriptionProductId,
                Products = new ProductsViewModel
                {
                    productId = osp.Product.productId,
                    productName = osp.Product.productName,
                    productPrice = osp.Product.productPrice
                },
                productQuantity = osp.productQuantity
            }).ToList(),

            intervalDays = o.intervalDays,
            orderDate = o.orderDate
        })
        .ToList();

    if (subscriptions == null || !subscriptions.Any())
    {
        return NotFound("No active subscriptions found for this user.");
    }

    return Ok(subscriptions);
}

            
        
        // dodanie subskrypcji
        [HttpPost("subscribe")]
        public async Task<IActionResult> AddSubscription([FromBody] OrderSubscriptionViewModel subscriptionViewModel)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("No valid user session.");
            }

            var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            // Pobranie produktu z bazy danych
            var product = _context.Products.SingleOrDefault(p => p.productId == subscriptionViewModel.OrderSubscriptionProducts.First().Products.productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Utworzenie nowej subskrypcji z tabelą pośrednią
            var newSubscription = new OrderSubscription
            {
                idUser = user.userId,
                intervalDays = subscriptionViewModel.intervalDays,
                orderDate = subscriptionViewModel.orderDate,
                OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
                {
                    new OrderSubscriptionProduct
                    {
                        productId = product.productId,
                        productQuantity = subscriptionViewModel.OrderSubscriptionProducts.First().productQuantity 
                    }
                }
            };

            _context.OrderSubscription.Add(newSubscription);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subscription added successfully." });
        }
        

            
        
        //edycja aktualnych subskrypcji
        [HttpPatch("subscriptions")]
    public async Task<IActionResult> PatchSubscription([FromBody] OrderSubscriptionViewModel subscriptionViewModel)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session.");
        }
        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        var subscription = _context.OrderSubscription
            .SingleOrDefault(o => o.orderSubscriptionId == subscriptionViewModel.orderSubscriptionId && o.idUser == user.userId);

        if (subscription == null)
        {
            return NotFound("Subscription not found.");
        }

        
        subscription.intervalDays = subscriptionViewModel.intervalDays;
        subscription.orderDate = subscriptionViewModel.orderDate;

        
        foreach (var productViewModel in subscriptionViewModel.OrderSubscriptionProducts)
        {
            var productInDb = _context.OrderSubscriptionProducts
                .SingleOrDefault(p => p.orderSubscriptionProductId == productViewModel.orderSubscriptionProductId);

            if (productInDb != null)
            {
                
                productInDb.productQuantity = productViewModel.productQuantity;

                var product = _context.Products.SingleOrDefault(p => p.productId == productViewModel.Products.productId);
                if (product != null)
                {
                    productInDb.Product = product;
                }
                else
                {
                    return NotFound($"Product with ID {productViewModel.Products.productId} not found.");
                }
            }
            else
            {
                
                subscription.OrderSubscriptionProducts.Add(new OrderSubscriptionProduct
                {
                    productId = productViewModel.Products.productId,
                    productQuantity = productViewModel.productQuantity,
                    orderSubscriptionId = subscription.orderSubscriptionId
                });
            }
        }

        _context.OrderSubscription.Update(subscription);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Subscription updated successfully." });
    }

        
        
        //anulowanie subskrypcji 
        
       
        [HttpDelete("subscriptions/{subscriptionId}")]
        [Authorize]
        public async Task<IActionResult> CancelSubscription(int subscriptionId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("No valid user session.");
            }

            var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }
    
            var subscription = _context.OrderSubscription
                .SingleOrDefault(o => o.orderSubscriptionId == subscriptionId && o.idUser == user.userId);

            if (subscription == null)
            {
                return NotFound("Subscription not found.");
            }
    
            _context.OrderSubscription.Remove(subscription);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subscription cancelled successfully." });
        }

        

        
        
    }
}
