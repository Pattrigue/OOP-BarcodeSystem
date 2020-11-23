﻿using System;
using System.Collections.Generic;
using BarcodeSystem.Controller;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.UI.Commands;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI
{
    public delegate void BarcodeSystemEvent(string command);

    public sealed class BarcodeSystemUI : IBarcodeSystemUI
    {
        public event BarcodeSystemEvent CommandEntered;

        private bool isRunning;

        private readonly IBarcodeSystemController controller;
        
        public BarcodeSystemUI(IBarcodeSystemController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            if (controller == null)
            {
                throw new NullReferenceException("Error: BarcodeSystemController has not been assigned to BarCodeSystemUI instance!");
            }
            
            BarcodeSystemCommandParser commandParser = new BarcodeSystemCommandParser(controller, this);
            
            isRunning = true;

            controller.UserBalanceWarning += DisplayUserBalanceWarning;
            
            do
            {
                ShowProducts(controller.ActiveProducts);

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

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user);
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"Too many arguments in command {command}!");
        }

        public void DisplayNotEnoughArgumentsError(string command)
        {
            Console.WriteLine($"Not enough arguments in command {command}!");
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

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine($"{transaction.User.Username} successfully purchased {count} {transaction.Product.Name}!");
        }

        public void DisplayInsufficientCash(User user, Product product)
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