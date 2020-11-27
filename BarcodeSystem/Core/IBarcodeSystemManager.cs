using System;
using System.Collections.Generic;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.Core
{
    public delegate void UserBalanceNotification(IUser user, decimal balance);
    
    public interface IBarcodeSystemManager
    {
        event UserBalanceNotification UserBalanceWarning;
        IEnumerable<IProduct> ActiveProducts { get; }
        BuyTransaction BuyProduct(IUser user, IProduct product, uint count);
        InsertCashTransaction AddCreditsToAccount(IUser user, decimal amount);
        IProduct GetProductById(uint productId);
        IEnumerable<IUser> GetUsers(Func<IUser, bool> predicate);
        IUser GetUserByUsername(string username);
        IEnumerable<ITransaction> GetTransactions(IUser user, int count);
    }
}