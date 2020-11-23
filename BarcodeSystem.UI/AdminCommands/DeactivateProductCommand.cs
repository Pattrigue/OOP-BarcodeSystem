namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class DeactivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => true;
    }
}