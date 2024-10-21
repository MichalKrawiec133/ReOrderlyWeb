using System.Diagnostics;
using ReOrderlyWeb.SQL.Data.DAO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Microsoft.EntityFrameworkCore;

namespace ReOrderlyWeb.SQL.Data.Migrations;

public class DatabaseSeed
{
    private readonly ReOrderlyWebDbContext _context;
    
    public DatabaseSeed(ReOrderlyWebDbContext context)
    {
        _context = context;
    }
    
    public void Seed()
    {
        var userList = BuildUserList();
        _context.User.AddRange(userList);
        _context.SaveChanges();
        
        var productsList = BuildProductsList();
        _context.Products.AddRange(productsList);
        _context.SaveChanges();
        
        var orderStatusList = BuildOrderStatusList();
        _context.OrderStatus.AddRange(orderStatusList);
        _context.SaveChanges();
        
        var orderList = BuildOrderList();
        _context.Order.AddRange(orderList);
        _context.SaveChanges();
        
        var orderItemsList = BuildOrderItemsList();
        _context.OrderItems.AddRange(orderItemsList);
        _context.SaveChanges();
        
        var orderSubscriptionList = BuildOrderSubscriptionList();
        _context.OrderSubscription.AddRange(orderSubscriptionList);
        _context.SaveChanges();
            
    }
    
    private IEnumerable<Products> BuildProductsList()
    {
        var productsList = new List<Products>();
        var products = new Products()
        {
            productId = 1,
            productName = "masło 82% 250g.",
            productPrice = 7.99,
            productQuantity = 35,
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 2,
            productName = "mleko 3,2% 1l.",
            productPrice = 4.99,
            productQuantity = 79,
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 3,
            productName = "jajka wiejskie 12szt.",
            productPrice = 14.99,
            productQuantity = 14,
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 4,
            productName = "kefir naturalny 250ml.",
            productPrice = 3.99,
            productQuantity = 12,
        };
        productsList.Add(products);
        
        return productsList;
    }
    
    private IEnumerable<User> BuildUserList()
    {
        var usersList = new List<User>();
        
        
        var pass = "nowak";
        string passmd5 = md5gen(pass);
        var user = new User()
        {
            userId = 1,
            name = "Marian",
            lastName = "Nowak",
            streetName = "Nowakowska",
            houseNumber = 12,
            voivodeship = "Nowakowskie",
            country = "Polska",
            zipcode = 11111,
            emailAddress = "mariannowak@wp.pl",
            password = passmd5,
            phoneNumber = 35123123,
        };
        usersList.Add(user);
        
        pass = "kawon";
        passmd5 = md5gen(pass);
        user = new User()
        {
            userId = 2,
            name = "Nairam",
            lastName = "Kawon",
            streetName = "Kawonowska",
            houseNumber = 89,
            voivodeship = "Kawonowskie",
            country = "Polska",
            zipcode = 22222,
            emailAddress = "nairamkawon@wp.pl",
            password = passmd5,
            phoneNumber = 909090909,
        };
        usersList.Add(user);
            
        pass = "ozimski";
        passmd5 = md5gen(pass);
        user = new User()
        {
            userId = 3,
            name = "Adam",
            lastName = "Ozimski",
            streetName = "Ozimska",
            houseNumber = 127,
            voivodeship = "Ozimskie",
            country = "Polska",
            zipcode = 33333,
            emailAddress = "adamozimski@wp.pl",
            password = passmd5,
            phoneNumber = 616161616,
        };
        usersList.Add(user);
        
        
        return usersList;
    }

    private IEnumerable<OrderStatus> BuildOrderStatusList()
    {
        var orderStatusList = new List<OrderStatus>();
        
        var orderStatus = new OrderStatus()
        {
            orderStatusId = 1,
            orderStatusDescription = "Opłacone"
           
        };
        orderStatusList.Add(orderStatus);
        
        orderStatus = new OrderStatus()
        {
            orderStatusId = 2,
            orderStatusDescription = "W trakcie przygotowania"
        };
        orderStatusList.Add(orderStatus);
        
        orderStatus = new OrderStatus()
        {
            orderStatusId = 3,
            orderStatusDescription = "Przekazane do doręczenia",
        };
        orderStatusList.Add(orderStatus);
        /*foreach (var status in orderStatusList)
        {
            Console.WriteLine($"ID: {status.orderStatusId}, Description: {status.orderStatusDescription}");
        }*/
        return orderStatusList;
    }

    private IEnumerable<Order> BuildOrderList()
    {
        DateTime localDate = DateTime.Now;
        var orderList = new List<Order>();
        var orders = new Order()
        {
            orderId = 1,
            idUser = 1,
            idOrderStatus = 1,
            orderDate = localDate,
        };
        orderList.Add(orders);
        
        localDate = DateTime.Now;
        orders = new Order()
        {
            orderId = 2,
            idUser = 2,
            idOrderStatus = 2,
            orderDate = localDate,
        };
        orderList.Add(orders);
        
        localDate = DateTime.Now;
        orders = new Order()
        {
            orderId = 3,
            idUser = 3,
            idOrderStatus = 3,
            orderDate = localDate,
        };
        orderList.Add(orders);
        orders = new Order()
        {
            orderId = 4,
            idUser = 1,
            idOrderStatus = 1,
            orderDate = localDate,
        };
        orderList.Add(orders);

        
        return orderList;
    }

    private IEnumerable<OrderItems> BuildOrderItemsList()
    {
        var orderItemsList = new List<OrderItems>();
        var orderItems = new OrderItems()
        {
            orderItemId = 1,
            idProduct = 2,
            idOrder = 1,
            orderItemQuantity = 3,
            orderPrice = 123,
        };
        orderItemsList.Add(orderItems);
        
        orderItems = new OrderItems()
        {
            orderItemId = 2,
            idProduct = 3,
            idOrder = 1,
            orderItemQuantity = 31,
            orderPrice = 321,
        };
        orderItemsList.Add(orderItems);
        
        orderItems = new OrderItems()
        {
            orderItemId = 3,
            idProduct = 1,
            idOrder = 2,
            orderItemQuantity = 5,
            orderPrice = 1123,
        };
        orderItemsList.Add(orderItems);
        
        orderItems = new OrderItems()
        {
            orderItemId = 4,
            idProduct = 3,
            idOrder = 3,
            orderItemQuantity = 12,
            orderPrice = 1523,
        };
        orderItemsList.Add(orderItems);
        
        orderItems = new OrderItems()
        {
            orderItemId = 5,
            idProduct = 1,
            idOrder = 4,
            orderItemQuantity = 85,
            orderPrice = 1523,
        };
        orderItemsList.Add(orderItems);
        
        orderItems = new OrderItems()
        {
            orderItemId = 6,
            idProduct = 4,
            idOrder = 4,
            orderItemQuantity = 85,
            orderPrice = 1523,
        };
        orderItemsList.Add(orderItems);
        
        return orderItemsList;
    }

    
    private IEnumerable<OrderSubscription> BuildOrderSubscriptionList()
    {
        var orderSubscriptionsList = new List<OrderSubscription>();
        
        var orderSubscriptions = new OrderSubscription()
        {
            orderSubscriptionId = 1,
            idUser = 1,
            idProduct = 1,
            productQuantity = 3,
            intervalDays = 3,
            orderDate = DateOnly.FromDateTime(DateTime.Now),
        };
        orderSubscriptionsList.Add(orderSubscriptions);

        orderSubscriptions = new OrderSubscription()
        {
            orderSubscriptionId = 2,
            idUser = 3,
            idProduct = 2,
            productQuantity = 2,
            intervalDays = 7,
            orderDate = DateOnly.FromDateTime(DateTime.Now),
        };
        orderSubscriptionsList.Add(orderSubscriptions);

        orderSubscriptions = new OrderSubscription()
        {
            orderSubscriptionId = 3,
            idUser = 2,
            idProduct = 3,
            productQuantity = 12,
            intervalDays = 14,
            orderDate = DateOnly.FromDateTime(DateTime.Now),
        };
        orderSubscriptionsList.Add(orderSubscriptions);
        
        orderSubscriptions = new OrderSubscription()
        {
            orderSubscriptionId = 4,
            idUser = 1,
            idProduct = 2,
            productQuantity = 12,
            intervalDays = 3,
            orderDate = DateOnly.FromDateTime(DateTime.Now),
        };
        orderSubscriptionsList.Add(orderSubscriptions);
        orderSubscriptions = new OrderSubscription()
        {
            orderSubscriptionId = 5,
            idUser = 1,
            idProduct = 3,
            productQuantity = 34,
            intervalDays = 31,
            orderDate = DateOnly.FromDateTime(DateTime.Now),
        };
        orderSubscriptionsList.Add(orderSubscriptions);
        return orderSubscriptionsList;
    }

    //TODO: ZMIENIC CALA BAZE Z ORDER SUBSCRIPTIONS - dodac posrednia tabele zawierajaca produkty zawarte w danej subskrypcji . :)
    private string md5gen(string pass)
    {
        using (var md5 = MD5.Create()) 
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(pass); 
            byte[] hashBytes = md5.ComputeHash(inputBytes); 
            string converted = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 
            //Console.WriteLine(converted); 
            return converted; 
        }
    }

    
}