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
        private const char AdminCommandPrefix = ':';
        
        private readonly IDashSystemController controller;
        private readonly IDashSystemUI dashSystemUI;

        private readonly Dictionary<string, IAdminCommand> adminCommands;

        public DashSystemCommandParser(IDashSystemController controller, IDashSystemUI dashSystemUI)
        {
            this.controller = controller;
            this.dashSystemUI = dashSystemUI;

            adminCommands = new Dictionary<string, IAdminCommand>()
            {
                { "addcredits", new AddCreditsToUserCommand() },
                { "qa", new ExitCommand() },
                { "activate", new ActivateProductCommand() },
                { "deactivate", new DeactivateProductCommand() },
                { "crediton", new SetProductCanBeBoughtOnCreditOn() },
                { "creditoff", new SetProductCanBeBoughtOnCreditOff() }
            };

            dashSystemUI.CommandEntered += ParseCommand;
        }

        private void ParseCommand(string command)
        {
            if (command.StartsWith(AdminCommandPrefix))
            {
                TryParseAdminCommand(command);
            }
            else
            {
                ParseUserCommand(command);
            }
        }

        private void TryParseAdminCommand(string command)
        {
            string[] args = command.Split(' ').Skip(1).ToArray();

            string adminCommandWithoutPrefix = command.Replace(AdminCommandPrefix.ToString(), string.Empty);
            
            foreach (string adminCommandString in adminCommands.Keys)
            {
                if (!adminCommandWithoutPrefix.StartsWith(adminCommandString)) continue;
                
                IAdminCommand adminCommand = adminCommands[adminCommandString];

                if (adminCommand.NumArguments != args.Length) continue;

                try
                {
                    adminCommand.Execute(args, dashSystemUI, controller);
                    adminCommand.DisplaySuccessMessage(dashSystemUI);
                    return;
                }
                catch (Exception e)
                {
                    dashSystemUI.DisplayError(e.Message);
                    return;
                }
            }
            
            dashSystemUI.DisplayAdminCommandNotFoundMessage(command);
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

            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return;
            }

            try
            {
                IUser user = controller.GetUserByUsername(username);
                IProduct product = controller.GetProductById(productId);

                controller.BuyProduct(user, product);
                Console.WriteLine($"User {user.Username} successfully purchased product {product.Name}!");         
            }
            catch (Exception e)
            {
                dashSystemUI.DisplayError(e.Message);
            };
        }
    }
}