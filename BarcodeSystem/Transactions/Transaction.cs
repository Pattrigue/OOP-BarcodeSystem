using System;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public abstract class Transaction : ITransaction
    {        
        protected Transaction(uint id, IUser user, DateTime date, decimal amount)
        {
            Id = id;
            User = user;
            Date = date;
            Amount = amount;
        }
        
        protected Transaction(IUser user, DateTime date, decimal amount)
        {
            Id = idCounter++;
            User = user;
            Date = date;
            Amount = amount;
        }

        private uint id;
        
        private static uint idCounter;

        protected abstract string CsvFileName { get; }

        public uint Id
        {
            get => id;
            set
            {
                if (value > idCounter)
                {
                    idCounter = value + 1;
                }

                id = value;
            }
        }

        public IUser User { get; }
        
        public DateTime Date { get; }
        
        public decimal Amount { get; }

        public override string ToString() => $"{Id} {User} {Amount} {Date}";

        public abstract void Execute();
        
        public abstract void Log(string dataDirectory);
    }
}