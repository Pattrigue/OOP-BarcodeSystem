using System;
using System.Collections.Generic;
using DashSystem.Data.Products;
using DashSystem.Data.Transactions;
using DashSystem.Data.Users;

namespace DashSystem.Data.Core
{
    public interface IDashSystem
    {
        event UserBalanceNotification UserBalanceWarning;
        IEnumerable<IProduct> ActiveProducts { get; }
        ITransaction BuyProduct(IUser user, IProduct product);
        ITransaction AddCreditsToAccount(IUser user, decimal amount);
        IProduct GetProductById(uint productId);
        IEnumerable<IUser> GetUsers(Func<IUser, bool> predicate);
        IUser GetUserByUsername(string username);
        IEnumerable<ITransaction> GetTransactions(IUser user, int count);
    }
}