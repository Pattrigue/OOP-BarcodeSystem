using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public abstract class SetProductActiveCommand : ProductAdminCommand
    {
        public override uint NumArguments => 1;

        protected abstract bool Active { get; }
        
        private IProduct product;
        
        public override void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            product = GetProduct(args[0], systemUI, systemManager);
            product.IsActive = Active;
        }

        public override void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage("Product {product.Name} has been activated!");
        }
    }
}