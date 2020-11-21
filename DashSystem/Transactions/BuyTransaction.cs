﻿using System;
using System.IO;
using DashSystem.Products;
using DashSystem.Users;

namespace DashSystem.Transactions
{
    public sealed class BuyTransaction : Transaction
    {
        public BuyTransaction(IUser user, IProduct product, DateTime date)
            : base(user, date, product.Price)
        {
            Product = product;
        }

        public override string CsvFileName => "buytransactions.csv";

        public IProduct Product { get; }

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