using System;
using DashSystem.Models.Users;

namespace DashSystem.Models.Transactions
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