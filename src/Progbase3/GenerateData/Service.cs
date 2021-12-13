using System;
using EntitiesRep;

namespace GenerateData
{
    public class Services
    {
        private UserRepository userRep;
        private ProductRepository productRep;
        private OrderRepository orderRep;

        public Services(UserRepository userRep, ProductRepository productRep, OrderRepository orderRep)
        {
            this.userRep = userRep;
            this.productRep = productRep;
            this.orderRep = orderRep;
        }

        public void AddOrder(long userId)
        {
            User user = new User();
            user = userRep.GetUserById(userId);
            if (user == null)
            {
                Console.WriteLine($"User with id : {userId} dosn`t exist.");
                return;
            }

            Console.WriteLine("Enter order information.");
            string orderInf = Console.ReadLine();
            string[] orderArr = orderInf.Split(' ');
            if (orderArr.Length < 1)
            {
                Console.WriteLine($"Incorrect input : {orderInf}.");
                return;
            }

            for (int i = 0; i < orderArr.Length; i++)
            {
                int parsed;
                if (!Int32.TryParse(orderArr[i], out parsed)) { Console.WriteLine($"Incorrect input (productID should be a number): '{orderInf}'"); 
                return;}
                Product orderProduct = productRep.GetProductById(long.Parse(orderArr[i]));
                if (orderProduct == null) { Console.WriteLine($"Product with id {orderArr[i]} doesn`t exist. Order isn`t created."); 
                return;}
                if (orderProduct.availability == false) { Console.WriteLine($"Product with id '{orderArr[i]}' not available. Please choose another products.");
                break;}
            }
            Order newOrder = new Order();
            newOrder.customerId = user.id;
            newOrder.orderDate = DateTime.Now;
            newOrder.amount = 0;
            for (int i = 0; i < orderArr.Length; i++)
            {
                Product orderProduct = productRep.GetProductById(long.Parse(orderArr[i]));
                newOrder.amount += orderProduct.price;
                if (i == 0)
                {
                    newOrder.id = orderRep.Insert(newOrder);
                    if (newOrder.id == 0)
                    {
                        Console.WriteLine($"Order doesn`t created.");
                        return; 
                    }

                }
                BuildOrderProductLink(orderProduct.id, newOrder.id, productRep, orderRep);
            }

            if (orderRep.Update(newOrder.id, newOrder))
            {
                Console.WriteLine($"Order created.");
            }
            return;
        }

        private void BuildOrderProductLink(long productId, long orderId, ProductRepository productRep, OrderRepository orderRep)
        {

            Product productForAdding = productRep.GetProductById(productId);
            if (productForAdding == null)
            {
                Console.WriteLine($"Product id '{productId}' doesn`t exist. Choose another product.");
                return;
            }
            if (productForAdding.availability == false)
            {
                Console.WriteLine($"Product with id '{productId}' not available. Please choose another product.");
                return;
            }

            if (!orderRep.AddProductConection(productForAdding.id, orderId))
            {
                Console.WriteLine("Connection NOT created!!!");
                return;
            }
            Console.WriteLine("Connection created.");


        }



    }
}
