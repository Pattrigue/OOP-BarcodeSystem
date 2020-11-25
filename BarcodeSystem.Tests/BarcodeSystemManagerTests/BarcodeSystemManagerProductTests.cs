using System.Collections.Generic;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using NUnit.Framework;

namespace BarcodeSystem.Tests.BarcodeSystemManagerTests
{
    public sealed class BarcodeSystemManagerProductTests
    {       
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
    }
}