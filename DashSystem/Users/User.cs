using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DashSystem.Users
{
    public sealed class User : IUser, IComparable<User>
    {
        private const int MaxUsernameCharacters = 9;

        private static uint idCounter = 0;

        private string email;
        private string firstName;
        private string lastName;
        private string username;
        
        private decimal balance;

        public User(string firstName, string lastName, string username, string email, decimal balance)
        {
            Id = idCounter++;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Balance = balance;
        }
        
        public uint Id { get; }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("First name cannot be empty or null!");
                }

                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Last name cannot be empty or null!");
                }

                lastName = value;
            }
        }
        
        public string Username
        {
            get => username;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Username cannot be empty or null!");
                }
                
                if (value.Length > MaxUsernameCharacters)
                {
                    throw new ArgumentException($"Username cannot contain more than {MaxUsernameCharacters} characters!");
                }
                
                if (Regex.IsMatch(value, "([^a-z0-9_])"))
                {
                    throw new ArgumentException("Usernames can only contain lowercase characters, numbers 0-9 and underscores!");
                }

                username = value;
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidEmailException("Email cannot be empty or null!", value);
                }

                string[] emailParts = value.Split('@');

                if (emailParts.Length != 2)
                {
                    throw new InvalidEmailException("Email format not recognized!", value);
                }

                string localPart = emailParts[0];
                string domain = emailParts[1];
                
                if (Regex.IsMatch(localPart, "[^a-zA-Z0-9._-]"))
                {
                    throw new InvalidEmailException("Email local part can only contain a-z, A-Z, and numbers 0-9 as well as dots, underscores and hyphens!", value);
                }

                if (Regex.IsMatch(domain, "[^a-zA-Z0-9.-]"))
                {
                    throw new InvalidEmailException("Email domain can only contain a-z, A-Z, and numbers 0-9 as well as dots and hyphens!", value);
                }

                if (domain.StartsWith('.') || domain.StartsWith('-'))
                {
                    throw new InvalidEmailException("Email domain cannot start with a dot or hyphen!", value);
                }

                if (domain.EndsWith('.') || domain.EndsWith('-'))
                {
                    throw new InvalidEmailException("Email domain cannot end with a dot or hyphen!", value);
                }
                
                if (!domain.Contains('.'))
                {
                    throw new InvalidEmailException("Email domain must contain at least one dot!", value);
                }
                
                email = value;
            }
        }

        public decimal Balance
        {
            get => balance;
            set
            {
                if (value < 50)
                {
                    Console.WriteLine("Warning: Balance is lower than 50 kr.!");
                }
                
                balance = value;
            }
        }

        public override string ToString() => $"{FirstName} {LastName} ({Email})";

        public override bool Equals(object obj)
        {
            if (obj is User otherUser)
            {
                return otherUser.Id == Id;
            }

            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public int CompareTo([NotNull] User otherUser) => Id.CompareTo(otherUser.Id);
    }
}