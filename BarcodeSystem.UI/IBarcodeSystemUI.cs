﻿using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI
{
    public interface IBarcodeSystemUI
    {
        event BarcodeSystemEvent CommandEntered;
        void Start();
        void Close();
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(IUser user);
        void DisplayTooManyArgumentsError(string command);
        void DisplayInvalidArgumentsError(string command, uint numArguments);
        void DisplayCommandNotFoundMessage(string command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(IUser user, IProduct product, uint count);
        void DisplayInsufficientCash(IUser user, IProduct product);
        void DisplayMessage(string message);
        void DisplayError(string errorMessage);
    }
}