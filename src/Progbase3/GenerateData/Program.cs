using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using EntitiesRep;

namespace GenerateData
{
    class Program
    {
        static void Main(string[] args)
        {
            Commond();
        }
        static void CheckInstructions(){
            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine("Command list:");
            Console.ForegroundColor=ConsoleColor.Magenta;
            Console.WriteLine("$[addItem] - add new item");
            Console.WriteLine("$[addOrder] - add new order");
            Console.WriteLine("$[addUser] - add new user");
            Console.WriteLine("$[getInfOrder] - get information about order");
            Console.WriteLine("$[deleteUser] - delete user by id");
            Console.WriteLine("$[getUserOrders] - get all orders of user");
            Console.WriteLine("$[generate products {num}] - generate products");
            Console.WriteLine("$[generate users {num}] - generate users");
            Console.WriteLine("$[exit] - close program");
            Console.ResetColor();
        }

        public static void Commond()
        {
            string dbPath = @"D:\inf\progbase3\data\mydb.db";
            SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
            UserRepository ur = new UserRepository(connection);
            ProductRepository gr = new ProductRepository(connection);
            OrderRepository or = new OrderRepository(connection);
            Services services = new Services(ur, gr, or);
            CheckInstructions();
            while (true)
            {
                Console.WriteLine("Enter command:");
                string input = Console.ReadLine();
                string[] inputArr = input.Split(" ");

                if (input == "addItem")
                {
                    //Console.WriteLine("Write item information like: {name} {price} {description} {availability: true/false}");
                    Product pr = new Product();
                    Console.WriteLine("Enter name of product:");
                    pr.productName = /*"new"*/Console.ReadLine();
                    Console.WriteLine("Enter price of product:");
                    pr.price = /*404*/Int32.Parse(Console.ReadLine());
                    //Console.WriteLine("Enter availability of product:");
                    pr.availability = true;
                    pr.createdAt = DateTime.Now;
                    gr.Insert(pr);
                    return;
                }
                else if (input == "addOrder")
                {
                    Console.WriteLine($"Enter userId: ");
                    long userId = 0;
                    try
                    {
                        userId = long.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Id must be a number.");
                        break;
                    }
                    services.AddOrder(userId);
                    return;

                }
                else if (input == "addUser")
                {

                    User user = new User();
                    bool state1 = true;
                    while (state1)
                    {
                        Console.WriteLine("Enter username:");
                        user.username = Console.ReadLine();
                        if (ur.CheckUserName(user.username) == 0)
                        {
                            Console.WriteLine("Enter fullname:");
                            user.fullname = Console.ReadLine();
                            bool state = true;
                            while (state)
                            {
                                Console.WriteLine("Enter status: (admin or customer)");
                                user.status = Console.ReadLine();
                                if (user.status == "admin" || user.status == "customer")
                                {
                                    ur.Insert(user);
                                    Console.WriteLine("User is added");
                                    state = false;
                                }
                                else
                                {
                                    Console.WriteLine("Incorect status, try again!");
                                }
                            }
                            state1 = false;
                        }
                        else
                        {
                            Console.WriteLine("This name is occupied by another user, try again!");
                        }
                    }

                    return;

                }
                else if (input == "getInfOrder")
                {
                    Console.WriteLine("Write order number");
                    long orderNum = 0;
                    try
                    {
                        orderNum = long.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine($"Order num must be a number.");
                        break;
                    }


                    Order order = or.GetOrderById(orderNum);
                    if (order == null)
                    {
                        Console.WriteLine($"Order with order number '{orderNum}' doesn`t exist.");
                        break;
                    }
                    Console.WriteLine($"Order: {orderNum}");
                    User customer = ur.GetUserById(order.customerId);
                    Console.WriteLine($"CustomerId: {customer.id}");
                    List<long> productIds = or.GetAllOrderProductsId(orderNum);
                    List<Product> prodList = gr.GetOrderProducts(productIds);
                    foreach (Product product in prodList)
                    {
                        Console.WriteLine(product.WriteForOrder());
                    }
                    return;
                }
                else if (input == "getUserOrders")
                {
                    long userId = 0;
                    try
                    {
                        userId = long.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine($"Order num must be a number.");
                        break;
                    }
                    User user = ur.GetUserById(userId);
                    if (user == null)
                    {
                        Console.WriteLine($"User with userId '{userId}' doesn`t exist.");
                        break;
                    }
                    Console.WriteLine($"User: {userId}");
                    List<long> ordersId = ur.GetAllUserOrdersId(userId);
                    List<Order> ordersList = or.GetAllUserOrdersById(ordersId);
                    if (ordersList.Count == 0)
                    {
                        Console.WriteLine($"User with id '{userId}' doesn`t have orders.");
                    }
                    foreach (Order order in ordersList)
                    {
                        Console.WriteLine(order.ToString());
                    }
                    return;
                }
                else if (input == "deleteUser")
                {
                    Console.WriteLine("Enter user's id: ");
                    long id = long.Parse(Console.ReadLine());
                    ur.DeleteById(id);
                    Console.WriteLine("Deleted");
                }

                else if (inputArr[0] == "generate")
                {
                    if (inputArr[1] == "products")
                    {
                        int numGoods = 0;
                        if (!Int32.TryParse(inputArr[2], out numGoods)) 
                        { 
                            Console.WriteLine($"Incorect items number input(must be int): {input}"); return; 
                        }

                        Generator.GenerateCargos(numGoods, connection);
                        Console.WriteLine("Generation is completed");
                    }
                    else if (inputArr[1] == "users")
                    {
                        int numUsers = 0;
                        if (!Int32.TryParse(inputArr[2], out numUsers))
                        {
                            Console.WriteLine($"Incorect items number input(must be int): {input}"); return; 
                        }

                        Generator.GenerateUsers(numUsers, connection);
                        Console.WriteLine("Generation is complited");
                    }
                    else
                    {
                        Console.Error.WriteLine($"Incorrect command, try again.");
                    }
                }
                else if (input == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Bye");
                    Console.ResetColor();
                    break;
                }

            }
        }
    }
}

