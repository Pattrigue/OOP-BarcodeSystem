namespace BarcodeSystem.UI.Commands.AdminCommands
{
    public sealed class SetProductCanBeBoughtOnCreditOn : SetProductCanBeBoughtOnCreditCommand
    {
        protected override bool CanBeBoughtOnCredit => true;
    }
}