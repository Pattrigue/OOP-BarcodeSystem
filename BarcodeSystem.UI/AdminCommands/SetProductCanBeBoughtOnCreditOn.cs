namespace BarcodeSystem.UI.AdminCommands
{
    public sealed class SetProductCanBeBoughtOnCreditOn : SetProductCanBeBoughtOnCreditCommand
    {
        protected override bool CanBeBoughtOnCredit => true;
    }
}