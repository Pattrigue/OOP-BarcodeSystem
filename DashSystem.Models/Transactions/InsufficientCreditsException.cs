using System;
using DashSystem.Models.Products;
using DashSystem.Models.Users;

namespace DashSystem.Models.Transactions
{
    public sealed class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(IProduct product, IUser user) : base(
            $"Insufficient credits to purchase product \"{product}\" for user \"{user}\"!") { }
    }
}