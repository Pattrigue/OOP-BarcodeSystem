using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public abstract class SetProductActiveCommand : ProductAdminCommand
    {
        public override uint NumArguments => 1;

        protected abstract bool Active { get; }
        
        private IProduct product;

        private string output;
        
        public override void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            product = GetProduct(args[0], systemUI, systemManager);
            
            if (Active == product.IsActive)
            { 
                output = Active
                    ? $"Product {product.Name} is already active!"
                    : $"Product {product.Name} is already inactive!";
            }
            else
            {
                output = Active
                    ? $"Product {product.Name} is now active."
                    : $"Product {product.Name} is no longer active.";
                
                product.IsActive = Active;
            }
        }

        public override void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage(output);
        }
    }
}