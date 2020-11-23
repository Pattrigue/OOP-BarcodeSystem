using System;
using System.Collections.Generic;
using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.UI.Commands.AdminCommands;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI.Commands
{
    public sealed class BarcodeSystemCommandParser
    {
        private const char AdminCommandPrefix = ':';
        
        private readonly IBarcodeSystemController controller;
        private readonly IBarcodeSystemUI barcodeSystemUI;

        private readonly Dictionary<string, IAdminCommand> adminCommands;

        public BarcodeSystemCommandParser(IBarcodeSystemController controller, IBarcodeSystemUI barcodeSystemUI)
        {
            this.controller = controller;
            this.barcodeSystemUI = barcodeSystemUI;

            adminCommands = new Dictionary<string, IAdminCommand>()
            {
                { "addcredits", new AddCreditsToUserCommand() },
                { "qa", new ExitCommand() },
                { "activate", new ActivateProductCommand() },
                { "deactivate", new DeactivateProductCommand() },
                { "crediton", new SetProductCanBeBoughtOnCreditOn() },
                { "creditoff", new SetProductCanBeBoughtOnCreditOff() }
            };

            barcodeSystemUI.CommandEntered += ParseCommand;
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
                    adminCommand.Execute(args, barcodeSystemUI, controller);
                    adminCommand.DisplaySuccessMessage(barcodeSystemUI);
                    return;
                }
                catch (Exception e)
                {
                    barcodeSystemUI.DisplayError(e.Message);
                    return;
                }
            }
            
            barcodeSystemUI.DisplayAdminCommandNotFoundMessage(command);
        }

        private void ParseUserCommand(string command)
        {
            string[] args = command.Split(' ');

            bool hasTooFewArguments = args.Length < 2;
            bool hasTooManyArguments = args.Length > 2;

            if (hasTooFewArguments)
            {
                barcodeSystemUI.DisplayNotEnoughArgumentsError(command);
                return;
            }
            
            if (hasTooManyArguments)
            {
                barcodeSystemUI.DisplayTooManyArgumentsError(command);
                return;
            }

            string username = args[0];
            string productIdString = args[1];

            if (!uint.TryParse(productIdString, out uint productId))
            {
                barcodeSystemUI.DisplayProductNotFound(productIdString);
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
                barcodeSystemUI.DisplayError(e.Message);
            };
        }
    }
}