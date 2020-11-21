using System;
using DashSystem.Products;
using DashSystem.Users;

namespace DashSystem.Transactions
{
    public sealed class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(IProduct product, IUser user) : base(
            $"Insufficient credits to purchase product \"{product}\" for user \"{user}\"!") { }
    }
}