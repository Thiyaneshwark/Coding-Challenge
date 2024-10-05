using System;
using System.Collections.Generic;
using OrderManagementSystem.dao;
using OrderManagementSystem.entity;
using OrderManagementSystem.exception;

namespace OrderManagementSystem.main
{
    class MainModule
    {
        static void Main(string[] args)
        {
            IOrderManagementRepository orderProcessor = new OrderProcessor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            CenterText("Welcome to the Order Management Application!");
            Console.ResetColor();

            while (true)
            {
                Console.WriteLine("\nAre you an Admin or a User? (Enter 'Admin' or 'User'): ");
                string userType = Console.ReadLine();

                if (userType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    if (!AuthenticateUser(orderProcessor, "Admin"))
                        continue;

                    AdminMenu(orderProcessor);
                }
                else if (userType.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    UserFlow(orderProcessor);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter 'Admin' or 'User'.");
                    Console.ResetColor();
                }
            }
        }

        private static bool AuthenticateUser(IOrderManagementRepository orderProcessor, string role)
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User user = orderProcessor.GetUserByUsername(username);
            if (user == null || user.Password != password || user.Role != role)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Authentication failed. Please check your credentials.");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        private static void UserFlow(IOrderManagementRepository orderProcessor)
        {
            while (true)
            {
                Console.WriteLine("\nAre you a New User or an Old User? (Enter 'New' or 'Old'): ");
                string userType = Console.ReadLine();

                if (userType.Equals("New", StringComparison.OrdinalIgnoreCase))
                {
                    RegisterUser(orderProcessor);
                    break;
                }
                else if (userType.Equals("Old", StringComparison.OrdinalIgnoreCase))
                {
                    bool loginSuccess = LoginUser(orderProcessor);
                    if (loginSuccess)
                    {
                        UserMenu(orderProcessor);
                        break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter 'New' or 'Old'.");
                    Console.ResetColor();
                }
            }
        }

        private static bool LoginUser(IOrderManagementRepository orderProcessor)
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User user = orderProcessor.GetUserByUsername(username);
            if (user == null || user.Password != password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Login failed. Please check your credentials.");
                Console.ResetColor();
                return false;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Welcome {user.Username}! You are logged in as {user.Role}.");
            Console.ResetColor();
            return true;
        }

        private static void AdminMenu(IOrderManagementRepository orderProcessor)
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1. Create Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. View Orders");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateProduct(orderProcessor);
                        break;
                    case 2:
                        ViewAllProducts(orderProcessor);
                        break;
                    case 3:
                        ViewOrdersByUser(orderProcessor);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void UserMenu(IOrderManagementRepository orderProcessor)
        {
            while (true)
            {
                Console.WriteLine("\nUser Menu:");
                Console.WriteLine("1. View All Products");
                Console.WriteLine("2. View Orders");
                Console.WriteLine("3. Cancel Order");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        ViewAllProducts(orderProcessor);
                        break;
                    case 2:
                        ViewOrdersByUser(orderProcessor);
                        break;
                    case 3:
                        CancelOrder(orderProcessor);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void RegisterUser(IOrderManagementRepository orderProcessor)
        {
            Console.Write("Enter a Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter a Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Role (User/Admin): ");
            string role = Console.ReadLine();

            if (!role.Equals("User", StringComparison.OrdinalIgnoreCase) && !role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid role. Please choose 'User' or 'Admin'.");
                return;
            }

            User newUser = new User { Username = username, Password = password, Role = role };
            orderProcessor.CreateUser(newUser);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful. You can now log in.");
            Console.ResetColor();
        }

        private static void CreateProduct(IOrderManagementRepository orderProcessor)
        {
            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine();
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());
            Console.Write("Enter Quantity in Stock: ");
            int quantityInStock = int.Parse(Console.ReadLine());
            Console.Write("Enter Product Type (Electronics/Clothing): ");
            string type = Console.ReadLine();

            if (type.Equals("Electronics", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Brand: ");
                string brand = Console.ReadLine();
                Console.Write("Enter Warranty Period (months): ");
                int warrantyPeriod = int.Parse(Console.ReadLine());

                Electronics electronics = new Electronics(0, productName, description, price, quantityInStock, brand, warrantyPeriod);
                orderProcessor.CreateProduct(null, electronics);
            }
            else if (type.Equals("Clothing", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter Size: ");
                string size = Console.ReadLine();
                Console.Write("Enter Color: ");
                string color = Console.ReadLine();

                Clothing clothing = new Clothing(0, productName, description, price, quantityInStock, size, color);
                orderProcessor.CreateProduct(null, clothing);
            }
            else
            {
                Console.WriteLine("Invalid product type.");
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Product created successfully.");
            Console.ResetColor();
        }

        private static void CancelOrder(IOrderManagementRepository orderProcessor)
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            try
            {
                orderProcessor.CancelOrder(userId, orderId);
                Console.WriteLine("Order cancelled successfully.");
            }
            catch (OrderNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ViewAllProducts(IOrderManagementRepository orderProcessor)
        {
            List<Product> products = orderProcessor.GetAllProducts();
            foreach (var product in products)
            {
                // Removed the "?" symbol and formatted the price
                Console.WriteLine($"ProductID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price:F2}, Stock: {product.QuantityInStock}, Type: {product.Type}");
            }
        }

        private static void ViewOrdersByUser(IOrderManagementRepository orderProcessor)
        {
            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());

            User user = new User { UserId = userId };

            List<Order> orders = orderProcessor.GetOrderByUser(user);
            if (orders.Count > 0)
            {
                Console.WriteLine($"Orders for User ID {userId}:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.OrderId}, Total Price: {order.TotalPrice:F2}");
                    Console.WriteLine("Products Ordered:");
                    foreach (var item in order.OrderItems)
                    {
                        Product product = orderProcessor.GetProductById(item.ProductId);
                        if (product != null)
                        {
                            Console.WriteLine($" - ProductID: {product.ProductId}, Name: {product.ProductName}, Description: {product.Description}, Price: {product.Price:F2}, Type: {product.Type}, Quantity: {item.Quantity}");
                        }
                        else
                        {
                            Console.WriteLine($" - Product with ID {item.ProductId} not found.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No orders found for this user.");
            }
        }

        private static void CenterText(string text)
        {
            int left = (Console.WindowWidth / 2) - (text.Length / 2);
            Console.SetCursorPosition(left < 0 ? 0 : left, Console.CursorTop);
            Console.WriteLine(text);
        }
    }
}
