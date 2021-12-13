using System;
using EntitiesRep;
using Microsoft.Data.Sqlite;

namespace GenerateData
{
    static class Generator
    {
        public static void GenerateCargos(int number, SqliteConnection connection)
        {
            string[] cargosArr = new string[] { "Mi Note 10 Lite", "Redmi 6A", "Mi8", "Pocophone f2 pro", "Mi 8 Lite", "Redmi 9", "Redmi Note 10 5G", "Redmi Note 10 Pro", 
            "Mi 11 Lite", "Mi 11i", "Redmi Note 10", "Black Shark 4", "Black Shark 3", "Mi 10T Lite", "Poco F3", "Redmi Note 9 Pro", "Mi 11", "Redmi Note 10S", "Mi 11 Ultra" };
            string[] boolean = new string[] { "true", "false" };
            Random random = new Random();
            ProductRepository productsRep = new ProductRepository(connection);
            for (int i = 0; i < number; i++)
            {
                Product newCargo = new Product();
                newCargo.productName = cargosArr[random.Next(0, cargosArr.Length - 1)];
                newCargo.price = random.Next(8500, 26000);
                newCargo.availability = Boolean.Parse(boolean[random.Next(0, 2)]);
                newCargo.createdAt = DateTime.Now;
                productsRep.Insert(newCargo);
            }
        }
        public static void GenerateUsers(int number, SqliteConnection connection)
        {
            string[] names = new string[] { "Cristiano", "Lionel", "Neymar", "Paul", "Alexis", "Marcus", "David", "Victor", "Phil", "Bruno", "Thomas", "Manuel", "Mark-Andre", "Karim", "Lorenzo", "Anthony", "Rud", "Marco", "Kevin", "Raheem", "Sergio", "Erling", "Kylian", "Bernardo", "Mayson" };
            string[] lastNames = new string[] { "Ronaldo", "Messi", "Junior", "Pogba", "Sanchez", "Rashford", "De Gea", "Lindelof", "Jones", "Fernandes", "Neuer", "Ter Stegen", "Benzema", "Insigne", "Martial", "Gullit", "Verrati", "De Bruyne", "Sterling", "Haaland", "Mbappe", "Silva", "Greenwood" };
            Random random = new Random();
            UserRepository usersRep = new UserRepository(connection);
            for (int i = 0; i < number; i++)
            {
                User newUser = new User();
                newUser.fullname = names[random.Next(0, names.Length - 1)] + " " + lastNames[random.Next(0, lastNames.Length - 1)];
                newUser.username = "user" + i.ToString();
                newUser.status = "customer";
                usersRep.Insert(newUser);
            }

        }
    }
}
