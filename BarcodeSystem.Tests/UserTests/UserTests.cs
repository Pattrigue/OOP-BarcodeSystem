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
        [TestCase("FirstName", "LastName", "CAPITAL USERNAME")]
        [TestCase("FirstName", "LastName", "()!")]
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
        
        [TestCase("username", "username", true)]
        [TestCase("username1", "username2", false)]
        public void UserEquals_CompareUsers_ExpectFalseIfUsernamesDiffer(string username1, string username2, bool expectAreEqual)
        {
            User user1 = CreateUser();
            User user2 = CreateUser();

            user1.Username = username1;
            user2.Username = username2;
            
            Assert.AreEqual(expectAreEqual, user1.Equals(user2));
        }
        
        [TestCase("string")]
        [TestCase(5)]
        public void UserEquals_CompareOtherTypes_ExpectFalse(object obj)
        {
            User user = CreateUser();
            
            Assert.AreEqual(false, user.Equals(obj));
        }

        [TestCase("abc", "def", -1)]
        [TestCase("def", "abc", 1)]
        [TestCase("abc", "abc", 0)]
        public void CompareTo_CompareUsers_ExpectInteger(string username1, string username2, int expectedValue)
        {
            User user1 = CreateUser();
            User user2 = CreateUser();

            user1.Username = username1;
            user2.Username = username2;
            
            Assert.AreEqual(expectedValue, user1.CompareTo(user2));
        }

        private static User CreateUser()
        {
            return new User(10, "First", "Last", "username", "email@example.com", 50m);
        }
    }
}