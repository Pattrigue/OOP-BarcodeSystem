using System;
using System.Collections.Generic;
using System.Linq;
using DashSystem.Core;
using DashSystem.Products;
using DashSystem.Users;

namespace DashSystem.UI
{
    public sealed class DashSystemCommandParser
    {
        private readonly IDashSystemController controller;
        private readonly IDashSystemUI dashSystemUI;

        private Dictionary<string, Action<string>> adminCommands;
        public DashSystemCommandParser(IDashSystemController controller, IDashSystemUI dashSystemUI)
        {
            this.controller = controller;
            this.dashSystemUI = dashSystemUI;

            adminCommands = new Dictionary<string, Action<string>>()
            {
                { ":q", _ => dashSystemUI.Close() },
                { ":activate", command =>
                    {
                        string[] args = command.Split(' ');

                        if (args.Length > 2)
                        {
                            dashSystemUI.DisplayTooManyArgumentsError(command);
                            return;
                        }

                        if (args.Length < 2)
                        {
                            dashSystemUI.DisplayTooManyArgumentsError(command);
                            return;
                        }

                        string productIdString = args[1];
                        
                        if (!uint.TryParse(args[1], out uint productId))
                        {
                            dashSystemUI.DisplayProductNotFound(productIdString);
                            return;
                        }
                        
                        IProduct product = controller.GetProductById(productId);
                        product.IsActive = true;
                        
                        dashSystemUI.DisplayMessage($"Product {product.Name} has been activated!");
                    } 
                }
            };

            dashSystemUI.CommandEntered += ParseCommand;
        }

        private void ParseCommand(string command)
        {
            if (ParseAdminCommand(command)) return;
            
            ParseUserCommand(command);
        }

        private bool ParseAdminCommand(string command)
        {
            foreach (string commandString in adminCommands.Keys.Where(command.StartsWith))
            {
                adminCommands[commandString].Invoke(command);
                return true;
            }

            return false;
        }

        private void ParseUserCommand(string command)
        {
            string[] args = command.Split(' ');

            bool hasTooFewArguments = args.Length < 2;
            bool hasTooManyArguments = args.Length > 2;
            
            if (hasTooFewArguments)
            {
                dashSystemUI.DisplayNotEnoughArgumentsError(command);
                return;
            }
            
            if (hasTooManyArguments)
            {
                dashSystemUI.DisplayTooManyArgumentsError(command);
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
                product = controller.GetProductById(productId);
            }
            catch (Exception e)
            {
                dashSystemUI.DisplayError(e.Message);
                return;
            };

            controller.BuyProduct(user, product);
            Console.WriteLine($"User {user.Username} successfully purchased product {product.Name}!");            
        }
    }
}