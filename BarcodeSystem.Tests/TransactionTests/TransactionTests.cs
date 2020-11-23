using System;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;
using NUnit.Framework;

namespace BarcodeSystem.Tests.TransactionTests
{
    public sealed class TransactionTests
    {
        [TestCase(50, 10)]
        [TestCase(90, 40)]
        [TestCase(500, 40)]
        public void Execute_UserBuysProduct_IsUserBalanceUpdatedCorrectly(decimal userBalance, decimal productPrice)
        {
            IUser user = CreateUser(userBalance);
            IProduct product = CreateProduct(productPrice);
            ITransaction transaction = new BuyTransaction(user, product, DateTime.Now);
            
            transaction.Execute();
            
            Assert.AreEqual(userBalance - productPrice, user.Balance);
        }

        [TestCase(50)]
        [TestCase(1000)]
        [TestCase(-50)]
        [TestCase(-1000)]
        public void Execute_UserInsertsCash_IsUserBalanceUpdatedCorrectly(decimal amount)
        {
            const decimal userBalance = 50;
            
            IUser user = CreateUser(userBalance);
            ITransaction transaction = new InsertCashTransaction(user, DateTime.Now, amount);

            transaction.Execute();

            Assert.AreEqual(userBalance + amount, user.Balance);
        } 
        
        private static User CreateUser(decimal userBalance)
        {
            return new User(10, "First", "Last", "username", "email@example.com", userBalance);
        }
        
        private static Product CreateProduct(decimal price)
        {
            return new Product(1, "Test", price, true);
        }
    }
}