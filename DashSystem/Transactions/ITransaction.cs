using System;
using DashSystem.Users;

namespace DashSystem.Transactions
{
    public interface ITransaction
    {
        uint Id { get; }
        IUser User { get; }
        DateTime Date { get; }
        decimal Amount { get; }
        void Execute();
    }
}