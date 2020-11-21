using System;
using DashSystem.Data.Products;
using DashSystem.Data.Users;

namespace DashSystem.Data.Transactions
{
    public sealed class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(IProduct product, IUser user) : base(
            $"Insufficient credits to purchase product \"{product}\" for user \"{user}\"!") { }
    }
}