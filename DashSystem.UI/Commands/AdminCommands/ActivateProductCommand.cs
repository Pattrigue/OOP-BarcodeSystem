namespace DashSystem.UI.Commands.AdminCommands
{
    public sealed class ActivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => false;
    }
}