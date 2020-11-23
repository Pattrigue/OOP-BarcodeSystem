using BarcodeSystem.Core;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class AddCreditsToUserCommand : IAdminCommand
    {
        public uint NumArguments => 2;

        private string username;
        private int amount;

        public void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            username = args[0];
            amount = int.Parse(args[1]);

            IUser user = systemManager.GetUserByUsername(username);
            systemManager.AddCreditsToAccount(user, amount);
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage($"Successfully added {amount} credits to {username}'s account!");
        }
    }
}