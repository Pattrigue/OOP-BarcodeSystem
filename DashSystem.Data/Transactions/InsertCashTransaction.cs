using System;
using System.IO;
using DashSystem.Data.Core;
using DashSystem.Data.Users;

namespace DashSystem.Data.Transactions
{
    public sealed class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(IUser user, DateTime date, decimal amount) 
            : base(user, date, amount) { }

        protected override string CsvFileName => Constants.InsertCashTransactionsFileName;

        public override string ToString() => $"Insert cash transaction with ID {Id}: {Amount}, {User}, {Date}";

        public override void Execute() => User.Balance += Amount;

        public override void Log(string dataDirectory)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(dataDirectory, CsvFileName)))
            {
                sw.WriteLine(string.Join(';', Id, User.Username, Date, Amount));
            }	
        }
    }
}