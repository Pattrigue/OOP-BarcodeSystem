using DashSystem.Core;

namespace DashSystem.UI.Commands.AdminCommands
{
    public interface IAdminCommand
    {
        int NumArguments { get; }
        void Execute(string[] args, IDashSystemUI dashSystemUI, IDashSystemController controller);
        void DisplaySuccessMessage(IDashSystemUI dashSystemUI);
    }
}