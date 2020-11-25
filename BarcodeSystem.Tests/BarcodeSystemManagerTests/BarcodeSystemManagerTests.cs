using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;
using NSubstitute.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BarcodeSystem.Tests.BarcodeSystemManagerTests
{
    public sealed class BarcodeSystemManagerTests
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
        public void GetProductById_GetFirstProduct_ExpectDoesNotThrow()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            
            Assert.DoesNotThrow(() => systemManager.GetProductById(1));
        }
        
        [Test]
        public void GetProductById_GetInvalidProduct_ExpectAssertion()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            
            Assert.Throws<ProductNotFoundException>(() => systemManager.GetProductById(0));
        }
        
        [Test]
        public void GetActiveProducts_GetAll_ExpectIsActive()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IEnumerable<IProduct> activeProducts = systemManager.ActiveProducts;

            foreach (IProduct product in activeProducts)
            {
                Assert.IsTrue(product.IsActive);
            }
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
        
        [Test]
        public void AddCreditsToAccount_BalanceChange_ExpectBalanceUpdated()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = systemManager.GetUsers((u => u.Username != null)).First();

            decimal amountToAdd = 50;
            decimal initialBalance = user.Balance;
            
            systemManager.AddCreditsToAccount(user, amountToAdd);
            
            Assert.AreEqual(initialBalance + amountToAdd, user.Balance);
        }
        
        [Test]
        public void BuyProduct_UserBuysProduct_ExpectInsufficientCreditsException()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = CreateUser(0);
            IProduct product = CreateProduct(100, false);

            Assert.Throws<InsufficientCreditsException>(() => systemManager.BuyProduct(user, product));
        }
        
        [TestCase(50, 50, false)]
        [TestCase(100, 50, false)]
        [TestCase(25, 50, true)]
        public void BuyProduct_UserBuysProduct_ExpectDoesNotThrowInsufficientCreditsException
            (decimal userBalance, decimal productPrice, bool canBeBoughtOnCredit)
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = CreateUser(userBalance);
            IProduct product = CreateProduct(productPrice, canBeBoughtOnCredit);

            Assert.DoesNotThrow(() => systemManager.BuyProduct(user, product));
        }
        
        private static User CreateUser(decimal balance)
        {
            return new User(1, "First", "Last", "username", "email@example.com", balance);
        }

        private static IProduct CreateProduct(decimal price, bool canBeBoughtOnCredit)
        {
            return new Product(1, "Product", 5m, true, canBeBoughtOnCredit);
        }
    }
}