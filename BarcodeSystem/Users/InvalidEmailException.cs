using System;

namespace BarcodeSystem.Users
{
    public sealed class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message, string email) : base($"Illegal email \"{email}\": {message}") { }
    }
}