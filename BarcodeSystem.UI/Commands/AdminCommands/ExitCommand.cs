using BarcodeSystem.Core;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public sealed class ExitCommand : IAdminCommand
    {
        public int NumArguments => 0;
        
        public void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemManager controller)
        {
            barcodeSystemUI.Close();
        }

        public void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI)
        {
            barcodeSystemUI.DisplayMessage("System closing.\nPress any key to exit the application.");
        }
    }
}