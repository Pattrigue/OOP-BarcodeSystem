using System;
using DashSystem.Models.Core;
using DashSystem.Models.Products;
using DashSystem.Models.Transactions;
using DashSystem.Models.Users;

namespace DashSystem.Models.CsvDataAccess
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

        public override ITransaction ToTransaction(IDashSystemController dashSystemController)
        {
            IUser user = dashSystemController.GetUserByUsername(Username);
            IProduct product = dashSystemController.GetProductById(productId);
                
            return new BuyTransaction(user, product, Date);
        }
    }
}