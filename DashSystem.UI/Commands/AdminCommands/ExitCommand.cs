using DashSystem.Core;

namespace DashSystem.UI.Commands.AdminCommands
{
    public sealed class ExitCommand : IAdminCommand
    {
        public int NumArguments => 0;
        
        public void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller)
        {
            dashSystemUI.Close();
        }

        public void DisplaySuccessMessage(IDashSystemUI dashSystemUI)
        {
            dashSystemUI.DisplayMessage("System closing.\nPress any key to exit the application.");
        }
    }
}