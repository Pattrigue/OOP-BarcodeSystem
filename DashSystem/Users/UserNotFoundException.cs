using System;

namespace DashSystem.Users
{
    public sealed class UserNotFoundException : Exception
    {
        public UserNotFoundException(string username) : base($"User with username {username} does not exist!") { }
    }
}