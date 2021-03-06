﻿using System;
using System.IO;
using BarcodeSystem.Core;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public sealed class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(IUser user, DateTime date, decimal amount) 
            : base(user, date, amount) { }

        public InsertCashTransaction(uint id, IUser user, DateTime date, decimal amount)
            : base(id, user,date, amount) { }

        protected override string CsvFileName => Constants.InsertCashTransactionsFileName;

        public override string ToString() => $"Insert cash transaction with ID = {Id}, Amount = {Amount}, User = {User.Username}, Date = {Date}";

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