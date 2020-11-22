using System;
using System.Collections.Generic;
using System.Linq;
using DashSystem.Core;
using DashSystem.Products;
using DashSystem.Users;

namespace DashSystem.UI
{
    public interface IAdminCommand
    {
        int NumArguments { get; }
        void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller);
        void DisplaySuccessMessage(IDashSystemUI dashSystemUI);
    }

    public sealed class ExitCommand : IAdminCommand
    {
        public int NumArguments => 0;
        
        public void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            dashSystemUI.Close();
        }

        public void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            dashSystemUI.DisplayMessage("System closing.\nPress any key to exit the application.");
        }
    }

    public sealed class AddCreditsToUserCommand : IAdminCommand
    {
        public int NumArguments => 2;

        private string username;
        private int amount;

        public void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            username = args[0];
            amount = int.Parse(args[1]);

            IUser user = controller.GetUserByUsername(username);
            controller.AddCreditsToAccount(user, amount);
        }

        public void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            dashSystemUI.DisplayMessage($"Successfully added {amount} credits to {username}'s account!");
        }
    }

    public abstract class ProductAdminCommand : IAdminCommand
    {
        public abstract int NumArguments { get; }

        public abstract void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller);

        public abstract void DisplaySuccessMessage(IDashSystemUI dashSystemUI);

        protected IProduct GetProduct(string productIdString, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return null;
            }
                        
            return controller.GetProductById(productId);
        }
    }

    public abstract class SetProductActiveCommand : ProductAdminCommand
    {
        public override int NumArguments => 1;

        protected abstract bool Active { get; }
        
        private IProduct product;
        
        public override void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            product = GetProduct(args[0], dashSystemUI, controller);
            product.IsActive = Active;
        }

        public override void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            Console.WriteLine("Product {product.Name} has been activated!");
        }
    }

    public sealed class DeactivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => true;
    }
    
    public sealed class ActivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => false;
    }

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
                { ":q", new ExitCommand() },
                { ":activate", new ActivateProductCommand() },
                { ":deactivate", new DeactivateProductCommand() },
                
                /*{ ":crediton", command => SetProductCanBeBoughtOnCredit(command, true) },
                { ":creditoff", command => SetProductCanBeBoughtOnCredit(command, false) },*/
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