﻿using System;
using DashSystem.Transactions;
using DashSystem.Users;

namespace DashSystem.CsvDataAccess
{
    public sealed class InsertCashTransactionCsvData : TransactionCsvData
    {
        public override void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            Id = uint.Parse(fields[0]);
            Username = fields[1];
            Date = DateTime.Parse(fields[2]);
            Amount = decimal.Parse(fields[3]);
        }

        public override ITransaction ToTransaction(IDashSystem dashSystem)
        {
            IUser user = dashSystem.GetUserByUsername(Username);

            return new InsertCashTransaction(user, Date, Amount);
        }
    }
}