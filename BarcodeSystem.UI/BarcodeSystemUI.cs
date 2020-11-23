using System;
using System.Collections.Generic;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI
{
    public delegate void BarcodeSystemEvent(string command);

    public sealed class BarcodeSystemUI : IBarcodeSystemUI
    {
        public event BarcodeSystemEvent CommandEntered;

        private bool isRunning;

        private readonly IBarcodeSystemManager systemManager;
        
        public BarcodeSystemUI(IBarcodeSystemManager systemManager)
        {
            this.systemManager = systemManager;
        }

        public void Start()
        {
            if (systemManager == null)
            {
                throw new NullReferenceException("Error: BarcodeSystemController has not been assigned to BarCodeSystemUI instance!");
            }
            
            isRunning = true;

            systemManager.UserBalanceWarning += DisplayUserBalanceWarning;
            
            do
            {
                ShowProducts(systemManager.ActiveProducts);

                string command = Console.ReadLine();
                CommandEntered?.Invoke(command);

                Console.ReadKey();
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
            Console.WriteLine($"Product {product} not found!");
        }

        public void DisplayUserInfo(IUser user)
        {
            Console.WriteLine(user);
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"Too many arguments in command {command}!");
        }

        public void DisplayCommandNotFoundMessage(string command)
        {
            Console.WriteLine($"Command {command} not found!");
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"Admin command {adminCommand} not found!");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine($"{transaction.User.Username} successfully purchased {transaction.Product.Name}!");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction, uint count)
        {
            Console.WriteLine($"{transaction.User.Username} successfully purchased {count} {transaction.Product.Name}!");
        }

        public void DisplayInsufficientCash(IUser user, IProduct product)
        {
            Console.WriteLine($"Insufficient cash!\nUser {user.Username} does not have enough cash to purchase {product.Name}");
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayError(string errorMessage)
        {
            Console.WriteLine($"Error: {errorMessage}");
        }

        private void DisplayUserBalanceWarning(IUser user, decimal balance)
        {
            Console.WriteLine($"{user.Username} has a low balance: {balance}.");
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