using DashSystem.Core;
using DashSystem.Users;

namespace DashSystem.UI.Commands.AdminCommands
{
    public sealed class AddCreditsToUserCommand : IAdminCommand
    {
        public int NumArguments => 2;

        private string username;
        private int amount;

        public void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            username = args[0];
            amount = int.Parse(args[1]);

            IUser user = controller.GetUserByUsername(username);
            controller.AddCreditsToAccount(user, amount);
        }

        public void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            dashSystemUI.DisplayMessage($"Successfully added {amount} credits to {username}'s account!");
        }
    }
}