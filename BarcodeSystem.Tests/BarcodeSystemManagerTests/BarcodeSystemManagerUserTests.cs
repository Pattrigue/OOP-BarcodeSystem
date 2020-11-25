using System.Collections.Generic;
using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Users;
using NUnit.Framework;

namespace BarcodeSystem.Tests.BarcodeSystemManagerTests
{
    public class BarcodeSystemManagerUserTests
    {
        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(null, 0)]
        public void GetUsers_GetByUsername_CountIsExpectedValue(int id, int expectedCount)
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IEnumerable<IUser> users = systemManager.GetUsers((user => user.Id == id));

            Assert.AreEqual(expectedCount, users.Count());
        }
        
        [Test]
        public void UserBalanceWarning_UserLowBalance_ExpectEventInvoked()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = systemManager.GetUsers((u => u.Username != null)).First();
            
            bool wasEventInvoked = false;

            systemManager.UserBalanceWarning += (user, balance) => wasEventInvoked = true; 
            user.Balance = 5;
            
            Assert.IsTrue(wasEventInvoked);
        }
    }
}