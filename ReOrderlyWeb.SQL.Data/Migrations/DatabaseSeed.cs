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
        
        /*var orderItemsList = BuildOrderItemsList();
        _context.OrderItems.AddRange(orderItemsList);
        _context.SaveChanges();*/
        
        var orderSubscriptionList = BuildOrderSubscriptionList();
        _context.OrderSubscription.AddRange(orderSubscriptionList);
        _context.SaveChanges();
        
        /*var orderSubscriptionProductList = BuildOrderSubscriptionProductList();
        _context.OrderSubscriptionProducts.AddRange(orderSubscriptionProductList);
        _context.SaveChanges();*/
            
    }
    
    private IEnumerable<Products> BuildProductsList()
    {
        var productsList = new List<Products>();
        var products = new Products()
        {
            productId = 1,
            productName = "Masło 82% 250g.",
            productPrice = 8.99,
            productQuantity = 35,
            imagePath = "/images/maslo.jpg",
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 2,
            productName = "Mleko 3,2% 1l.",
            productPrice = 4.99,
            productQuantity = 79,
            imagePath = "/images/mleko.jpg",
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 3,
            productName = "Jajka wiejskie 12szt.",
            productPrice = 14.99,
            productQuantity = 14,
            imagePath = "/images/jajka.jpg"
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 4,
            productName = "Chleb swojski 1kg",
            productPrice = 9.99,
            productQuantity = 50,
            imagePath = "/images/chleb.jpg"
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 5,
            productName = "Kiełbaski grillowe surowe 1kg",
            productPrice = 25.99,
            productQuantity = 15,
            imagePath = "/images/kielbasy.jpg"
        };
        productsList.Add(products);
        
        products = new Products()
        {
            productId = 6,
            productName = "Szynka z indyka plastry 100g",
            productPrice = 5.99,
            productQuantity = 50,
            imagePath = "/images/szynka_indyk.jpg"
        };
        productsList.Add(products);
        products = new Products()
        {
            productId = 7,
            productName = "Szynka plastry mix 100g",
            productPrice = 7.99,
            productQuantity = 50,
            imagePath = "/images/szynka_mix.jpg"
        };
        productsList.Add(products);
        products = new Products()
        {
            productId = 8,
            productName = "Salami plastry 100g",
            productPrice = 6.99,
            productQuantity = 50,
            imagePath = "/images/szynka_salami.jpg"
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
        var orderList = new List<Order>
        {
            new Order
            {
                orderId = 1,
                idUser = 1,
                idOrderStatus = 1,
                orderDate = DateTime.Now,
                OrderItems = new List<OrderItems>
                {
                    new OrderItems { idProduct = 2, orderItemQuantity = 3, orderPrice = 14.97 },
                    new OrderItems { idProduct = 3, orderItemQuantity = 1, orderPrice = 8.99 }
                }
            },
            new Order
            {
                orderId = 2,
                idUser = 2,
                idOrderStatus = 2,
                orderDate = DateTime.Now,
                OrderItems = new List<OrderItems>
                {
                    new OrderItems { idProduct = 1, orderItemQuantity = 5, orderPrice = 44.95 }
                }
            },
            new Order
            {
                orderId = 3,
                idUser = 3,
                idOrderStatus = 3,
                orderDate = DateTime.Now,
                OrderItems = new List<OrderItems>
                {
                    new OrderItems { idProduct = 3, orderItemQuantity = 12, orderPrice = 179.88 }
                }
            },
            new Order
            {
                orderId = 4,
                idUser = 1,
                idOrderStatus = 1,
                orderDate = DateTime.Now,
                OrderItems = new List<OrderItems>
                {
                    new OrderItems { idProduct = 5, orderItemQuantity = 2, orderPrice = 51.98 },
                    new OrderItems { idProduct = 7, orderItemQuantity = 5, orderPrice = 39.95 }
                }
            }
        };

        return orderList;
    }

    /*
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
    */

    
    private IEnumerable<OrderSubscription> BuildOrderSubscriptionList()
{
    var orderSubscriptionsList = new List<OrderSubscription>();

    var orderSubscriptions = new OrderSubscription()
    {
        orderSubscriptionId = 1,
        idUser = 1,
        intervalDays = 3,
        orderDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-5),
        OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
        {
            new OrderSubscriptionProduct { productId = 1, productQuantity = 3 },
            new OrderSubscriptionProduct { productId = 2, productQuantity = 4 }
        }
    };
    orderSubscriptionsList.Add(orderSubscriptions);

    orderSubscriptions = new OrderSubscription()
    {
        orderSubscriptionId = 2,
        idUser = 3,
        intervalDays = 7,
        orderDate = DateOnly.FromDateTime(DateTime.Now),
        OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
        {
            new OrderSubscriptionProduct { productId = 2, productQuantity = 5 }
        }
    };
    orderSubscriptionsList.Add(orderSubscriptions);

    orderSubscriptions = new OrderSubscription()
    {
        orderSubscriptionId = 3,
        idUser = 2,
        intervalDays = 14,
        orderDate = DateOnly.FromDateTime(DateTime.Now),
        OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
        {
            new OrderSubscriptionProduct { productId = 3, productQuantity = 2 }
        }
    };
    orderSubscriptionsList.Add(orderSubscriptions);

    orderSubscriptions = new OrderSubscription()
    {
        orderSubscriptionId = 4,
        idUser = 1,
        intervalDays = 3,
        orderDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-5),
        OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
        {
            new OrderSubscriptionProduct { productId = 1, productQuantity = 4 }
        }
    };
    orderSubscriptionsList.Add(orderSubscriptions);
    
    orderSubscriptions = new OrderSubscription()
    {
        orderSubscriptionId = 5,
        idUser = 1,
        intervalDays = 6,
        orderDate = DateOnly.FromDateTime(DateTime.Now),
        OrderSubscriptionProducts = new List<OrderSubscriptionProduct>
        {
            new OrderSubscriptionProduct { productId = 3, productQuantity = 2 }
        }
    };
    orderSubscriptionsList.Add(orderSubscriptions);

    return orderSubscriptionsList;
}



    /*
    private IEnumerable<OrderSubscriptionProduct> BuildOrderSubscriptionProductList()
    {
        var orderSubscriptionProductList = new List<OrderSubscriptionProduct>();

        var orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 1,
            orderSubscriptionId = 1,
            productId = 1,
            productQuantity = 3
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);

        orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 2,
            orderSubscriptionId = 2,
            productId = 2,
            productQuantity = 5
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);

        orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 3,
            orderSubscriptionId = 3,
            productId = 3,
            productQuantity = 2
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);

        orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 4,
            orderSubscriptionId = 4,
            productId = 1,
            productQuantity = 4
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);
        orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 5,
            orderSubscriptionId = 5,
            productId = 3,
            productQuantity = 2
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);

        orderSubscriptionProduct = new OrderSubscriptionProduct()
        {
            orderSubscriptionProductId = 6,
            orderSubscriptionId = 1,
            productId = 2,
            productQuantity = 4
        };
        orderSubscriptionProductList.Add(orderSubscriptionProduct);

        return orderSubscriptionProductList;
    }
    */
    
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