using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;
using NUnit.Framework;

namespace BarcodeSystem.Tests.BarcodeSystemManagerTests
{
    public class BarcodeSystemManagerTransactionTests
    {
        [Test]
        public void BuyProduct_UserBuysProduct_ExpectTransactionProductEqualsProduct()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = CreateUser(100);
            IProduct product = CreateProduct(100, false);

            BuyTransaction transaction = systemManager.BuyProduct(user, product);
            
            Assert.AreEqual(product, transaction.Product);
        }
        
        [Test]
        public void BuyProduct_UserBuysProduct_ExpectTransactionUserEqualsUser()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = CreateUser(100);
            IProduct product = CreateProduct(100, false);

            BuyTransaction transaction = systemManager.BuyProduct(user, product);
            
            Assert.AreEqual(user, transaction.User);
        }
        
        [Test]
        public void BuyProduct_UserBuysProduct_ExpectTransactionAmountEqualsProductPrice()
        {
            const decimal productPrice = 100;
            
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = CreateUser(100);
            IProduct product = CreateProduct(productPrice, false);

            BuyTransaction transaction = systemManager.BuyProduct(user, product);
            
            Assert.AreEqual(productPrice, transaction.Amount);
        }
        
        [Test]
        public void AddCreditsToAccount_BalanceChange_ExpectBalanceUpdated()
        {
            BarcodeSystemManager systemManager = new BarcodeSystemManager();
            IUser user = systemManager.GetUsers(u => u.Username != null).First();

            const decimal amountToAdd = 50;
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
            return new Product(1, "Product", price, true, canBeBoughtOnCredit);
        }
    }
}