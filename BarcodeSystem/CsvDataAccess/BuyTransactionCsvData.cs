using System;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.CsvDataAccess
{
    public sealed class BuyTransactionCsvData : TransactionCsvData
    {
        private uint productId;

        public override void ReadLine(char separator, string csvLine)
        {
            string[] fields = csvLine.Split(separator);

            Id = uint.Parse(fields[0]);
            Username = fields[1];
            productId = uint.Parse(fields[2]);
            Date = DateTime.Parse(fields[3]);
            Amount = decimal.Parse(fields[4]);
        }

        public override ITransaction ToTransaction(IBarcodeSystemManager barcodeSystemManager)
        {
            IUser user = barcodeSystemManager.GetUserByUsername(Username);
            IProduct product = barcodeSystemManager.GetProductById(productId);
                
            return new BuyTransaction(Id, user, product, Date);
        }
    }
}