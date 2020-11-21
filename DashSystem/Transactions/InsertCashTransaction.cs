using System;
using DashSystem.Users;

namespace DashSystem.Transactions
{
    public sealed class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(IUser user, DateTime date, decimal amount) 
            : base(user, date, amount) { }

        public override string ToString() => $"Insert cash transaction with ID {Id}: {Amount}, {User}, {Date}";

        public override void Execute() => User.Balance += Amount;
    }
}