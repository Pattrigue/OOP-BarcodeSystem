using System;
using BarcodeSystem.Core;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.CsvDataAccess
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

        public override ITransaction ToTransaction(IBarcodeSystemManager barcodeSystemManager)
        {
            IUser user = barcodeSystemManager.GetUserByUsername(Username);

            return new InsertCashTransaction(user, Date, Amount);
        }
    }
}