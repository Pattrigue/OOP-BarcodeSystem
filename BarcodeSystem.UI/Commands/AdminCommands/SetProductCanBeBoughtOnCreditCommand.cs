using BarcodeSystem.Core;
using BarcodeSystem.Products;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public abstract class SetProductCanBeBoughtOnCreditCommand : IAdminCommand
    {
        public int NumArguments => 1;

        protected abstract bool CanBeBoughtOnCredit { get; }

        private IProduct product;
        
        public void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemController controller)
        {
            uint productId = uint.Parse(args[0]);

            product = controller.GetProductById(productId);
            product.CanBeBoughtOnCredit = CanBeBoughtOnCredit;
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI)
        {
            barcodeSystemUI.DisplayMessage($"Product {product.Name} can be bought on credit = {CanBeBoughtOnCredit}.");
        }
    }
}