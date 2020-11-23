using System;
using System.IO;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public sealed class BuyTransaction : Transaction
    {
        public BuyTransaction(IUser user, IProduct product, DateTime date)
            : base(user, date, product.Price)
        {
            Product = product;
        }

        public IProduct Product { get; }

        protected override string CsvFileName => Constants.BuyTransactionsFileName;

        public override string ToString() => $"Buy transaction with ID {Id}: {Amount}, {User}, {Date}";

        public override void Execute()
        {
            if (!Product.CanBeBoughtOnCredit && User.Balance - Amount < 0)
            {
                throw new InsufficientCreditsException(Product, User);
            }

            User.Balance -= Amount;
        }

        public override void Log(string dataDirectory)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(dataDirectory, CsvFileName)))
            {
                sw.WriteLine(string.Join(';', Id, User.Username, Product.Id, Date, Amount));
            }	
        }
    }
}