namespace DashSystem.UI.Commands.AdminCommands
{
    public sealed class DeactivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => true;
    }
}