using System;
using BarcodeSystem.Products;
using NUnit.Framework;

namespace BarcodeSystem.Tests.ProductTests
{
    public sealed class ProductTests
    {
        [TestCase(1U)]
        [TestCase(50U)]
        [TestCase(5000U)]
        [TestCase(uint.MaxValue)]
        public void SetId_SetTo1OrPositive_ExpectNoException(uint id)
        { 
            Product product = CreateProduct();

            Assert.DoesNotThrow(() => product.Id = id);
        }
        
        [Test]
        public void SetId_SetTo0OrNegative_ExpectArgumentException()
        { 
            Product product = CreateProduct();

            Assert.Throws<ArgumentException>(() => product.Id = 0);
        }

        [Test]
        public void SetName_SetToNull_ExpectArgumentException()
        {
            Product product = CreateProduct(); 

            Assert.Throws<ArgumentException>(() => product.Name = null);
        }

        private static Product CreateProduct()
        {
            return new Product(1, "Test", 50m, true);
        }
    }
}