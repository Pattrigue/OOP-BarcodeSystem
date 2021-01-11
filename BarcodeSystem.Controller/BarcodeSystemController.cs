using System;
using System.Collections.Generic;
using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.UI;
using BarcodeSystem.UI.AdminCommands;
using BarcodeSystem.Users;

namespace BarcodeSystem.Controller
{
    public sealed class BarcodeSystemController
    {
        private const char AdminCommandPrefix = ':';
        
        private readonly IBarcodeSystemManager systemManager;
        private readonly IBarcodeSystemUI systemUI;

        private readonly Dictionary<string, IAdminCommand> adminCommands;

        public BarcodeSystemController(IBarcodeSystemManager systemManager, IBarcodeSystemUI systemUI)
        {
            this.systemManager = systemManager;
            this.systemUI = systemUI;

            adminCommands = new Dictionary<string, IAdminCommand>()
            {
                { "addcredits", new AddCreditsToUserCommand() },
                { "qa", new ExitCommand() },
                { "activate", new SetProductActiveCommand(true) },
                { "deactivate", new SetProductActiveCommand(false) },
                { "crediton", new SetProductCanBeBoughtOnCredit(true) },
                { "creditoff", new SetProductCanBeBoughtOnCredit(false) }
            };

            systemUI.CommandEntered += ParseCommand;
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

                if (adminCommand.NumArguments != args.Length)
                {
                    systemUI.DisplayInvalidArgumentsError(adminCommandString, adminCommand.NumArguments);
                    return;
                }

                try
                {
                    adminCommand.Execute(args, systemUI, systemManager);
                    adminCommand.DisplaySuccessMessage(systemUI);
                    return;
                }
                catch (Exception e)
                {
                    systemUI.DisplayError(e.Message);
                    return;
                }
            }
            
            systemUI.DisplayAdminCommandNotFoundMessage(command);
        }

        private void ParseUserCommand(string command)
        {
            string[] args = command.Split(' ');
            
            switch (args.Length)
            {
                case 1:
                    DisplayUserInfo(args[0]);
                    break;
                case 2:
                    BuyProduct(args[0], args[1], 1);
                    break;
                case 3:
                    BuyProduct(args[0], args[1], args[2]);
                    break;
                default:
                    systemUI.DisplayTooManyArgumentsError(command);
                    break;
            }
        }

        private void DisplayUserInfo(string username)
        {
            try
            {
                systemUI.DisplayUserInfo(systemManager.GetUserByUsername(username));
            }
            catch
            {
                systemUI.DisplayUserNotFound(username);
            }
        }

        private void BuyProduct(string username, string productIdString, string countString)
        {
            if (!uint.TryParse(countString, out uint count))
            {
                systemUI.DisplayError($"Unrecognized number: {countString}");
                return;
            }
            
            BuyProduct(username, productIdString, count);
        }
        
        private void BuyProduct(string username, string productIdString, uint count)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                systemUI.DisplayProductNotFound(productIdString);
                return;
            }

            if (count < 1)
            {
                systemUI.DisplayError($"Invalid product count: {count}.");
                return;
            }
            
            try
            {
                IUser user = systemManager.GetUserByUsername(username);
                IProduct product = systemManager.GetProductById(productId);

                systemManager.BuyProduct(user, product, count);
                systemUI.DisplayUserBuysProduct(user, product, count);
            }
            catch (Exception e)
            {
                systemUI.DisplayError(e.Message);
            };
        }
    }
}