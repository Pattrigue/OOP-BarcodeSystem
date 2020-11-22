using DashSystem.Core;
using DashSystem.Products;

namespace DashSystem.UI.Commands.AdminCommands
{
    public abstract class SetProductCanBeBoughtOnCreditCommand : IAdminCommand
    {
        public int NumArguments => 1;

        protected abstract bool CanBeBoughtOnCredit { get; }

        private IProduct product;
        
        public void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            uint productId = uint.Parse(args[0]);

            product = controller.GetProductById(productId);
            product.CanBeBoughtOnCredit = CanBeBoughtOnCredit;
        }

        public void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            dashSystemUI.DisplayMessage($"Product {product.Name} can be bought on credit = {CanBeBoughtOnCredit}.");
        }
    }
}