using System;
using System.Collections.Generic;
using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.UI;
using BarcodeSystem.UI.Commands.AdminCommands;
using BarcodeSystem.Users;

namespace BarcodeSystem.Controller
{
    public sealed class BarcodeSystemController
    {
        private const char AdminCommandPrefix = ':';
        
        private readonly IBarcodeSystemManager systemManager;
        private readonly IBarcodeSystemUI ui;

        private readonly Dictionary<string, IAdminCommand> adminCommands;

        public BarcodeSystemController(IBarcodeSystemManager systemManager, IBarcodeSystemUI ui)
        {
            this.systemManager = systemManager;
            this.ui = ui;

            adminCommands = new Dictionary<string, IAdminCommand>()
            {
                { "addcredits", new AddCreditsToUserCommand() },
                { "qa", new ExitCommand() },
                { "activate", new ActivateProductCommand() },
                { "deactivate", new DeactivateProductCommand() },
                { "crediton", new SetProductCanBeBoughtOnCreditOn() },
                { "creditoff", new SetProductCanBeBoughtOnCreditOff() }
            };

            ui.CommandEntered += ParseCommand;
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
                    adminCommand.Execute(args, ui, systemManager);
                    adminCommand.DisplaySuccessMessage(ui);
                    return;
                }
                catch (Exception e)
                {
                    ui.DisplayError(e.Message);
                    return;
                }
            }
            
            ui.DisplayAdminCommandNotFoundMessage(command);
        }

        private void ParseUserCommand(string command)
        {
            string[] args = command.Split(' ');

            bool hasTooFewArguments = args.Length < 2;
            bool hasTooManyArguments = args.Length > 2;

            string username = args[0];
            string productIdString = args[1];
            
            if (hasTooFewArguments)
            {
                ui.DisplayNotEnoughArgumentsError(command);
                return;
            }
            
            if (hasTooManyArguments)
            {
                ui.DisplayTooManyArgumentsError(command);
                return;
            }


            if (!uint.TryParse(productIdString, out uint productId))
            {
                ui.DisplayProductNotFound(productIdString);
                return;
            }

            try
            {
                IUser user = systemManager.GetUserByUsername(username);
                IProduct product = systemManager.GetProductById(productId);
                
                systemManager.BuyProduct(user, product);
                Console.WriteLine($"User {user.Username} successfully purchased product {product.Name}!");         
            }
            catch (Exception e)
            {
                ui.DisplayError(e.Message);
            };
        }
    }
}