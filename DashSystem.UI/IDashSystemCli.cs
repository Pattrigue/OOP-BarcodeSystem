﻿using DashSystem.Products;
using DashSystem.Transactions;
using DashSystem.Users;

namespace DashSystem.UI
{
    public interface IDashSystemCli
    {
        event DashSystemEvent CommandEntered;
        void Start();
        void Close();
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(User user);
        void DisplayTooManyArgumentsError(string command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count, BuyTransaction transaction);
        void DisplayInsufficientCash(User user, Product product);
        void DisplayGeneralError(string errorString);
    }
}