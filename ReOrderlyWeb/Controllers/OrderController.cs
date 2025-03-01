using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;
    

[Authorize]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public OrderController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }

    // pobranie wszystkich zamowien
    [HttpGet("orders")]
    public IActionResult GetOrders()
    {
        
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session.");
        }

        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var orders = _context.Order
            .Where(o => o.idUser == user.userId)
            .Select(o => new OrderViewModel
            {
                orderId = o.orderId,
                idUser = o.idUser,
                OrderStatus = new OrderStatusViewModel 
                {
                    orderStatusId = o.OrderStatus.orderStatusId,
                    orderStatusDescription = o.OrderStatus.orderStatusDescription
                },
                orderDate = o.orderDate,
                orderItems = _context.OrderItems
                    .Where(oi => oi.idOrder == o.orderId)
                    .Select(oi => new OrderItemsViewModel
                    {
                        orderItemId = oi.orderItemId,
                        Products = new ProductsViewModel  
                        {
                            productId = oi.Products.productId,
                            productName = oi.Products.productName,
                            productPrice = oi.Products.productPrice,
                            productQuantity = oi.Products.productQuantity,
                            imagePath = oi.Products.imagePath 
                        },
                        idOrder = oi.idOrder,
                        orderItemQuantity = oi.orderItemQuantity,
                        orderPrice = oi.orderPrice
                    })
                    .ToList()
            })
            .ToList();

        return Ok(orders);
    }


    
    // dodaj nowe zamowienie
    [HttpPost("checkout")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel orderViewModel)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session.");
        }

        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (orderViewModel == null)
        {
            return BadRequest("Order data is null.");
        }

        if (orderViewModel.orderItems == null || !orderViewModel.orderItems.Any())
        {
            return BadRequest("Order items are missing.");
        }

        var order = new Order
        {
            idUser = user.userId,
            idOrderStatus = orderViewModel.OrderStatus.orderStatusId,  
            orderDate = orderViewModel.orderDate
        };
        
        foreach (var item in orderViewModel.orderItems)
        {
            var orderItem = new OrderItems
            {
                idProduct = item.Products.productId,
                orderItemQuantity = item.orderItemQuantity,
                orderPrice = item.orderPrice
            };

            order.OrderItems.Add(orderItem); 
       
            var product = await _context.Products.FindAsync(item.Products.productId);
            if (product == null)
            {
                return NotFound($"Product with ID {item.Products.productId} not found.");
            }

            if (product.productQuantity < item.orderItemQuantity)
            {
                return BadRequest($"Not enough stock for product ID {item.Products.productId}. Available: {product.productQuantity}, Requested: {item.orderItemQuantity}");
            }

            product.productQuantity -= item.orderItemQuantity;
            order.OrderItems.Add(orderItem); 
        }
        _context.Order.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order and items created successfully", orderId = order.orderId });
    }




}


    
