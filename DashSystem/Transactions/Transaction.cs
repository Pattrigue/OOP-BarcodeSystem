using System;
using DashSystem.Users;

namespace DashSystem.Transactions
{
    public abstract class Transaction : ITransaction
    {        
        protected Transaction(IUser user, DateTime date, decimal amount)
        {
            Id = idCounter++;
            User = user;
            Date = date;
            Amount = amount;
        }
        
        private static uint idCounter;

        protected abstract string CsvFileName { get; }

        public uint Id { get; }

        public IUser User { get; }
        
        public DateTime Date { get; }
        
        public decimal Amount { get; }

        public override string ToString() => $"{Id} {User} {Amount} {Date}";

        public abstract void Execute();
        
        public abstract void Log(string dataDirectory);
    }
}