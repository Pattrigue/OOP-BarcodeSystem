namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class SetProductCanBeBoughtOnCreditOff : SetProductCanBeBoughtOnCreditCommand
    {
        protected override bool CanBeBoughtOnCredit => false;
    }
}