using BarcodeSystem.Core;

namespace BarcodeSystem.UI.AdminCommands
{
    public interface IAdminCommand
    {
        int NumArguments { get; }
        void Execute(string[] args, IBarcodeSystemUI systemUI, IBarcodeSystemManager systemManager);
        void DisplaySuccessMessage(IBarcodeSystemUI systemUI);
    }
}