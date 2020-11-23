using System;
using BarcodeSystem.Products;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public sealed class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(IProduct product, IUser user) : base(
            $"Insufficient credits to purchase product \"{product.Name}\" for user \"{user.Username}\"!") { }
    }
}