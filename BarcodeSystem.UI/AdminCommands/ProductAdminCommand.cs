using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public abstract class ProductAdminCommand : IAdminCommand
    {
        public abstract uint NumArguments { get; }

        public abstract void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager);

        public abstract void DisplaySuccessMessage(IBarcodeSystemUI systemUI);

        protected IProduct GetProduct(string productIdString, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                systemUI.DisplayProductNotFound(productIdString);
                return null;
            }
                        
            return systemManager.GetProductById(productId);
        }
    }
}