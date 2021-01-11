using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class SetProductActiveCommand : ProductAdminCommand
    {
        public override uint NumArguments => 1;

        private IProduct product;

        private string output;

        private readonly bool active;

        public SetProductActiveCommand(bool active)
        {
            this.active = active;
        }
        
        public override void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            product = GetProduct(args[0], systemUI, systemManager);
            
            if (active == product.IsActive)
            { 
                output = active
                    ? $"Product {product.Name} is already active!"
                    : $"Product {product.Name} is already inactive!";
            }
            else
            {
                output = active
                    ? $"Product {product.Name} is now active."
                    : $"Product {product.Name} is no longer active.";
                
                product.IsActive = active;
            }
        }

        public override void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage(output);
        }
    }
}