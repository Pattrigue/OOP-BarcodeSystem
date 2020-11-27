using System;
using System.IO;
using BarcodeSystem.Core;
using BarcodeSystem.Products;
using BarcodeSystem.Users;

namespace BarcodeSystem.Transactions
{
    public sealed class BuyTransaction : Transaction
    {
        public BuyTransaction(IUser user, IProduct product, DateTime date, uint count = 1)
            : base(user, date, product.Price)
        {
            Product = product;
            this.count = count;
        }

        public BuyTransaction(uint id, IUser user, IProduct product, DateTime date, uint count = 1)
            : this(user, product, date, count)
        {
            Id = id;
        }

        public IProduct Product { get; }

        protected override string CsvFileName => Constants.BuyTransactionsFileName;

        private readonly uint count = 1u;
        
        public override string ToString() => $"Buy transaction with ID = {Id}, Amount = {Amount}, User = {User}, Date = {Date}, Count = {count}";

        public override void Execute()
        {
            if (count == 0) return;

            decimal totalPrice = Amount * count;
            
            if (!Product.CanBeBoughtOnCredit && User.Balance - totalPrice < 0)
            {
                throw new InsufficientCreditsException(Product, User);
            }

            User.Balance -= totalPrice;
        }

        public override void Log(string dataDirectory)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(dataDirectory, CsvFileName)))
            {
                sw.WriteLine(string.Join(';', Id, User.Username, Product.Id, Date, Amount, count));
            }	
        }
    }
}