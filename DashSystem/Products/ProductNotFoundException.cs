using System;

namespace DashSystem.Products
{
    public sealed class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(uint productId) : base($"Product with ID {productId} does not exist!") { }
    }
}