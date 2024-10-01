using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.ViewModels;
using System.Security.Claims;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.Controllers
{
    
    [ApiController]
    public class OrderSubscriptionController : ControllerBase
    {
        private readonly ReOrderlyWebDbContext _context;

        public OrderSubscriptionController(ReOrderlyWebDbContext context)
        {
            _context = context;
        }
        
        //wyswietlenie wszystkich subskrypcji
        [HttpGet("subscriptions")]
        [Authorize] 
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

            //pobranie z bazy wszystkich subskrypcji dla danego usera. 
            var subscriptions = _context.OrderSubscription
                .Where(o => o.idUser == user.userId)
                .Select(o => new OrderSubscriptionViewModel
                {
                    orderSubscriptionId = o.orderSubscriptionId,
                    idUser = o.idUser,
                    User = new UserViewModel  // Możesz tutaj dodać, jeśli nawigujesz do User
                    {
                        userId = o.User.userId,
                        lastName = o.User.lastName,
                        emailAddress = o.User.emailAddress
                    },
                    Products = new ProductsViewModel 
                    {
                        productId = o.Products.productId,
                        productName = o.Products.productName,
                        productPrice = o.Products.productPrice
                    },
                    productQuantity = o.productQuantity,
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

           
            var newSubscription = new OrderSubscription
            {
                idUser = user.userId,
                Products = new Products
                {
                    productName = subscriptionViewModel.Products.productName,
                    productPrice = subscriptionViewModel.Products.productPrice
                },
                productQuantity = subscriptionViewModel.productQuantity,
                intervalDays = subscriptionViewModel.intervalDays,
                orderDate = subscriptionViewModel.orderDate
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
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int idUser = int.Parse(userIdClaim);
            
            var user = _context.User.SingleOrDefault(c => c.userId == idUser);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

           
            var patchSubscription = new OrderSubscription
            {
                orderSubscriptionId = subscriptionViewModel.orderSubscriptionId,
                idUser = user.userId,
                Products = new Products
                {
                    productName = subscriptionViewModel.Products.productName,
                    productPrice = subscriptionViewModel.Products.productPrice
                },
                productQuantity = subscriptionViewModel.productQuantity,
                intervalDays = subscriptionViewModel.intervalDays,
                orderDate = subscriptionViewModel.orderDate
            };

            
            _context.OrderSubscription.Update(patchSubscription);
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
