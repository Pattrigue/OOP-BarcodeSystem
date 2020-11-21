using System;
using DashSystem.Data.Core;
using DashSystem.Data.Products;
using DashSystem.Data.Transactions;
using DashSystem.Data.Users;

namespace DashSystem.Data.CsvDataAccess
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

        public override ITransaction ToTransaction(IDashSystem dashSystem)
        {
            IUser user = dashSystem.GetUserByUsername(Username);
            IProduct product = dashSystem.GetProductById(productId);
                
            return new BuyTransaction(user, product, Date);
        }
    }
}