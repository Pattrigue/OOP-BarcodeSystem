using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public abstract class ProductAdminCommand : IAdminCommand
    {
        public abstract int NumArguments { get; }

        public abstract void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemManager controller);

        public abstract void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI);

        protected IProduct GetProduct(string productIdString, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemManager controller)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                barcodeSystemUI.DisplayProductNotFound(productIdString);
                return null;
            }
                        
            return controller.GetProductById(productId);
        }
    }
}