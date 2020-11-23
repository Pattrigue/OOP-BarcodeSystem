namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class ActivateProductCommand : SetProductActiveCommand
    {
        protected override bool Active => false;
    }
}