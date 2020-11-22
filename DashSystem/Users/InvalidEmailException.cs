using System;

namespace DashSystem.Users
{
    public sealed class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message, string email) : base($"Illegal email \"{email}\": {message}") { }
    }
}