using System;
using BarcodeSystem.Users;
using NUnit.Framework;

namespace BarcodeSystem.Tests.UserTests
{
    public sealed class UserTests
    {
        [TestCase(null)]
        [TestCase("email")]
        [TestCase("email@example")]
        [TestCase("email@-example.com")]
        [TestCase("email@example.com_")]
        [TestCase("email@example.com_")]
        [TestCase("email@example.com_")]
        [TestCase("email@.example.com")]
        [TestCase("email@example.com.")]
        [TestCase("(invalid)@example.com")]
        public void SetEmail_SetToIncorrectFormat_ExpectAssertion(string email)
        {
            User user = CreateUser();

            Assert.Throws<InvalidEmailException>(() => user.Email = email);
        }

        [TestCase("FirstName", "LastName", null)]
        [TestCase("FirstName", null, "username")]
        [TestCase(null, "LastName", "username")]
        public void SetFirstNameLastNameUserName_SetToNull_ExpectAssertion(string firstName, string lastName, string username)
        {
            User user = CreateUser();

            Assert.Throws<ArgumentException>(() =>
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Username = username;
            });
        }

        private static User CreateUser()
        {
            return new User(10, "First", "Last", "username", "email@example.com", 50m);
        }
    }
}