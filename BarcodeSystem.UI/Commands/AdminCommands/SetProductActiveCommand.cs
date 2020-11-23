using System;
using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public abstract class SetProductActiveCommand : ProductAdminCommand
    {
        public override int NumArguments => 1;

        protected abstract bool Active { get; }
        
        private IProduct product;
        
        public override void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemController controller)
        {
            product = GetProduct(args[0], barcodeSystemUI, controller);
            product.IsActive = Active;
        }

        public override void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI)
        {
            Console.WriteLine("Product {product.Name} has been activated!");
        }
    }
}