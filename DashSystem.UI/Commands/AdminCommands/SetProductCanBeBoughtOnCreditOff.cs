namespace DashSystem.UI.Commands.AdminCommands
{
    public sealed class SetProductCanBeBoughtOnCreditOff : SetProductCanBeBoughtOnCreditCommand
    {
        protected override bool CanBeBoughtOnCredit => false;
    }
}