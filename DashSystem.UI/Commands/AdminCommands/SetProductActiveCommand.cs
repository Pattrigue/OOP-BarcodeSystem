using System;
using DashSystem.Core;
using DashSystem.Products;

namespace DashSystem.UI.Commands.AdminCommands
{
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
}