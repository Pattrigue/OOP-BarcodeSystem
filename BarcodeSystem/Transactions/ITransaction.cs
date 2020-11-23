using System;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public interface ITransaction
    {
        uint Id { get; }
        IUser User { get; }
        DateTime Date { get; }
        decimal Amount { get; }
        void Execute();
        void Log(string dataDirectory);
    }
}