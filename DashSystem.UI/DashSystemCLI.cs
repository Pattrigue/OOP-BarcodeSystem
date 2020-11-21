using System;
using System.Collections.Generic;
using DashSystem.Core;
using DashSystem.Models.Core;
using DashSystem.Products;
using DashSystem.Transactions;
using DashSystem.Users;

namespace DashSystem.UI
{
    public delegate void DashSystemEvent(string command);
        
    public sealed class DashSystemCli : IDashSystemCli
    {
        public event DashSystemEvent CommandEntered;

        private bool isRunning;

        public void Start()
        {
            IDashSystemController controller = new DashSystemController();
          
            isRunning = true;

            do
            {
                ShowProducts(controller.ActiveProducts);

                Console.ReadLine();
                Console.Clear();
            } while (isRunning);
        }

        public void Close()
        {
            isRunning = false;
        }

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User {username} not found!");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"User {product} not found!");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user);
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"Too many arguments in command {command}!");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"Admin command {adminCommand} not found!");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine($"Insufficient cash!\nUser {user.Username} does not have enough cash to purchase {product.Name}");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"Error: {errorString}");
        }

        private void ShowProducts(IEnumerable<IProduct> products)
        {
            foreach (IProduct product in products)
            {
                Console.WriteLine(product);
            }

            Console.WriteLine("");
        }
    }
}