using DashSystem.Core;
using DashSystem.Products;

namespace DashSystem.UI.Commands.AdminCommands
{
    public abstract class ProductAdminCommand : IAdminCommand
    {
        public abstract int NumArguments { get; }

        public abstract void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller);

        public abstract void DisplaySuccessMessage(IDashSystemUI dashSystemUI);

        protected IProduct GetProduct(string productIdString, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            if (!uint.TryParse(productIdString, out uint productId))
            {
                dashSystemUI.DisplayProductNotFound(productIdString);
                return null;
            }
                        
            return controller.GetProductById(productId);
        }
    }
}