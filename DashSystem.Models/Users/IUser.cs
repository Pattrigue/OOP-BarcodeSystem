namespace DashSystem.Models.Users
{
    public interface IUser
    {
        event LowFundsNotification LowFundsWarning;
        uint Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        decimal Balance { get; set; }
    }
}