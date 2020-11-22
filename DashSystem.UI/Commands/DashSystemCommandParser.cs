using System;
using System.Collections.Generic;
using System.Linq;
using DashSystem.Core;
using DashSystem.Products;
using DashSystem.UI.Commands.AdminCommands;
using DashSystem.Users;

namespace DashSystem.UI.Commands
{
    public sealed class DashSystemCommandParser
    {
        private readonly IDashSystemController controller;
        private readonly IDashSystemUI dashSystemUI;

        private readonly Dictionary<string, IAdminCommand> adminCommands;

        public DashSystemCommandParser(IDashSystemController controller, IDashSystemUI dashSystemUI)
        {
            this.controller = controller;
            this.dashSystemUI = dashSystemUI;

            adminCommands = new Dictionary<string, IAdminCommand>()
            {
                { ":addcredits", new AddCreditsToUserCommand() },
                { ":qa", new ExitCommand() },
                { ":activate", new ActivateProductCommand() },
                { ":deactivate", new DeactivateProductCommand() },
                { ":crediton", new SetProductCanBeBoughtOnCreditOn() },
                { ":creditoff", new SetProductCanBeBoughtOnCreditOff() }
            };

            dashSystemUI.CommandEntered += ParseCommand;
        }

        private void ParseCommand(string command)
        {
            if (TryParseAdminCommand(command)) return;
            
            ParseUserCommand(command);
        }

        private bool TryParseAdminCommand(string command)
        {
            string[] args = command.Split(' ').Skip(1).ToArray();

            foreach (string commandString in adminCommands.Keys.Where(command.StartsWith))
            {
                IAdminCommand adminCommand = adminCommands[commandString];

                if (adminCommand.NumArguments != args.Length) continue;
                
                try
                {
                    adminCommand.Execute(args, dashSystemUI, controller);
                    adminCommand.DisplaySuccessMessage(dashSystemUI);
                    return true;
                }
                catch (Exception e)
                {
                    dashSystemUI.DisplayError(e.Message);
                }
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
        
        private void SetProductActive(string productIdString, bool active)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return;
            }
                        
            IProduct product = controller.GetProductById(productId);
            product.IsActive = active;
            
            dashSystemUI.DisplayMessage($"Product {product.Name} has been {(active ? "activated" : "deactivated")}!");
        }
        
        private void SetProductCanBeBoughtOnCredit(string productIdString, bool canBeBoughtOnCredit)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return;
            }
                        
            IProduct product = controller.GetProductById(productId);
            product.CanBeBoughtOnCredit = canBeBoughtOnCredit;
            
            dashSystemUI.DisplayMessage($"Product {product.Name} can be bought on credit = {canBeBoughtOnCredit}.");
        }

        private void AddCredits(string userIdString)
        {
            
        }
    }
}