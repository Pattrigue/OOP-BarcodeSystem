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
            systemUI.DisplayMessage($"Product {product.Name} can be bought on credit = {CanBeBoughtOnCredit}.");
        }
    }
}