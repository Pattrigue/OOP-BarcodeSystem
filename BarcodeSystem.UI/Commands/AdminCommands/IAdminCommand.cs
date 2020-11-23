using BarcodeSystem.Core;

namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public interface IAdminCommand
    {
        int NumArguments { get; }
        void Execute(string[] args, IBarcodeSystemUI barcodeSystemUI, IBarcodeSystemController controller);
        void DisplaySuccessMessage(IBarcodeSystemUI barcodeSystemUI);
    }
}