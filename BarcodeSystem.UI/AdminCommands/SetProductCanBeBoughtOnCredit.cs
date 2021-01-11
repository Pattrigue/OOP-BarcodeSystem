using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class SetProductCanBeBoughtOnCredit : IAdminCommand
    {
        private IProduct product;

        private string output;

        private readonly bool canBeBoughtOnCredit;

        public SetProductCanBeBoughtOnCredit(bool canBeBoughtOnCredit)
        {
            this.canBeBoughtOnCredit = canBeBoughtOnCredit;
        }

        public uint NumArguments => 1;

        public void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            uint productId = uint.Parse(args[0]);

            product = systemManager.GetProductById(productId);
            
            if (canBeBoughtOnCredit == product.CanBeBoughtOnCredit)
            { 
                output = canBeBoughtOnCredit
                    ? $"Product {product.Name} can already be bought on credit!"
                    : $"Product {product.Name} already cannot be bought on credit!";
            }
            else
            {
                output = canBeBoughtOnCredit
                    ? $"Product {product.Name} can now be bought on credit."
                    : $"Product {product.Name} can no longer be bought on credit.";
                
                product.CanBeBoughtOnCredit = canBeBoughtOnCredit;
            }
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage(output);
        }
    }
}