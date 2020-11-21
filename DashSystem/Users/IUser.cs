namespace DashSystem.Users
{
    public interface IUser
    {
        uint Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        decimal Balance { get; set; }
    }
}