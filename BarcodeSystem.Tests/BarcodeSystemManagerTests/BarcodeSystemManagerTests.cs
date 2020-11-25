using System;
using System.Collections.Generic;
using System.Linq;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Users;
using NUnit.Framework;

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
            BarcodeSystemManager sys = new BarcodeSystemManager();
            IEnumerable<IUser> users = sys.GetUsers((user => user.Id == id));

            Assert.AreEqual(expectedCount, users.Count());
        }
        
        [Test]
        public void GetProductById_GetFirstProduct_ExpectDoesNotThrow()
        {
            BarcodeSystemManager sys = new BarcodeSystemManager();
            
            Assert.DoesNotThrow(() => sys.GetProductById(1));
        }
        
        [Test]
        public void GetProductById_GetInvalidProduct_ExpectAssertion()
        {
            BarcodeSystemManager sys = new BarcodeSystemManager();
            
            Assert.Throws<ProductNotFoundException>(() => sys.GetProductById(0));
        }
    }
}