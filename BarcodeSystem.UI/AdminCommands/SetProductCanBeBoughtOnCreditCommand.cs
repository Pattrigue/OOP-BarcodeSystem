using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.AdminCommands
{
    public abstract class SetProductCanBeBoughtOnCreditCommand : IAdminCommand
    {
        public uint NumArguments => 1;

        protected abstract bool CanBeBoughtOnCredit { get; }

        private IProduct product;
        
        public void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            uint productId = uint.Parse(args[0]);

            product = systemManager.GetProductById(productId);
            product.CanBeBoughtOnCredit = CanBeBoughtOnCredit;
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            string output;

            if (product.CanBeBoughtOnCredit == CanBeBoughtOnCredit)
            {
                output = CanBeBoughtOnCredit
                    ? $"Product {product.Name} can already be bought on credit!"
                    : $"Product {product.Name} already cannot be bought on credit.";
            }
            else
            {
                output = CanBeBoughtOnCredit
                    ? $"Product {product.Name} can now be bought on credit!"
                    : $"Product {product.Name} can no longer be bought on credit.";
            }
            
            systemUI.DisplayMessage(output);
        }
    }
}