﻿using System;
using System.Collections.Generic;
using DashSystem.Products;
using DashSystem.Transactions;
using DashSystem.Users;

namespace DashSystem.Core
{
    public delegate void UserBalanceNotification(IUser user, decimal balance);
    
    public interface IDashSystemController
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