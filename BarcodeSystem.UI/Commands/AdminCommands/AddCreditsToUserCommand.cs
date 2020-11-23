using BarcodeSystem.Core;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public sealed class AddCreditsToUserCommand : IAdminCommand
    {
        public int NumArguments => 2;

        private string username;
        private int amount;

        public void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemManager controller)
        {
            username = args[0];
            amount = int.Parse(args[1]);

            IUser user = controller.GetUserByUsername(username);
            controller.AddCreditsToAccount(user, amount);
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI)
        {
            barcodeSystemUI.DisplayMessage($"Successfully added {amount} credits to {username}'s account!");
        }
    }
}