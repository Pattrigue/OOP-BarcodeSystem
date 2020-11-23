namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public sealed class DeactivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => true;
    }
}