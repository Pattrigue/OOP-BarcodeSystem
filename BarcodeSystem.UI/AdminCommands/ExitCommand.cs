using BarcodeSystem.Core;

namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class ExitCommand : IAdminCommand
    {
        public uint NumArguments => 0;
        
        public void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager)
        {
            systemUI.Close();
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI systemUI)
        {
            systemUI.DisplayMessage("System closing.\nPress any key to exit the application.");
        }
    }
}