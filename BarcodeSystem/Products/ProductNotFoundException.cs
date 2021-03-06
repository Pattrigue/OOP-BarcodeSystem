﻿using System;

namespace BarcodeSystem.Products
{
    public sealed class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(uint productId) : base($"Product with ID {productId} does not exist!") { }
    }
}