using System;
using DashSystem.Core;
using DashSystem.Products;
using DashSystem.Users;

namespace DashSystem.UI
{
    public sealed class DashSystemCommandParser
    {
        private readonly IDashSystemController controller;
        private readonly IDashSystemUI dashSystemUI;
        
        public DashSystemCommandParser(IDashSystemController controller, IDashSystemUI dashSystemUI)
        {
            this.controller = controller;
            this.dashSystemUI = dashSystemUI;

            dashSystemUI.CommandEntered += ParseCommand;
        }

        private void ParseCommand(string command)
        {
            string[] args = command.Split(' ');
            bool isCommandFormatValid = args.Length == 2;

            if (!isCommandFormatValid)
            {
                dashSystemUI.DisplayCommandNotFoundMessage(command);
                return;
            }

            string username = args[0];
            string productIdString = args[1];

            IUser user;
            IProduct product;

            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return;
            }
            
            try
            {
                user = controller.GetUserByUsername(username);
            }
            catch (Exception e)
            {
                dashSystemUI.DisplayError(e.Message);
                return;
            };

            try
            {
                product = controller.GetProductById(productId);
            }
            catch (Exception e)
            {
                dashSystemUI.DisplayError(e.Message);
                return;
            }

            controller.BuyProduct(user, product);
            Console.WriteLine($"User {user.Username} successfully purchased product {product.Name}!");
        }
    }
}