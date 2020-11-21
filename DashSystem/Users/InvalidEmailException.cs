using System;

namespace DashSystem.Users
{
    public sealed class InvalidEmailException : Exception
    {
        public string Email { get; }
        
        public InvalidEmailException() { }

        public InvalidEmailException(string message) : base(message) { }

        public InvalidEmailException(string message, Exception inner) : base(message, inner) { }

        public InvalidEmailException(string message, string email) : base($"Illegal email \"{email}\": {message}") { }
    }
}