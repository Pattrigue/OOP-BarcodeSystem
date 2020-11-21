using System;
using System.Collections.Generic;
using DashSystem.Models.Core;
using DashSystem.Models.Products;
using DashSystem.Models.Transactions;
using DashSystem.Models.Users;

namespace DashSystem.UI
{
    public sealed class DashSystemCli : IDashSystemCli
    {
        public void Start()
        {
            IDashSystemController controller = new DashSystemController();

            ShowProducts(controller.ActiveProducts);

            Console.ReadLine(); 
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void DisplayUserNotFound(string username)
        {
            throw new NotImplementedException();
        }

        public void DisplayProductNotFound(string product)
        {
            throw new NotImplementedException();
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user);
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            throw new NotImplementedException();
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DisplayGeneralError(string errorString)
        {
            throw new NotImplementedException();
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