using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DashSystem.Models.CsvDataAccess;
using DashSystem.Models.Products;
using DashSystem.Models.Transactions;
using DashSystem.Models.Users;

namespace DashSystem.Models.Core
{
    public delegate void UserBalanceNotification(IUser user, decimal balance);

    public sealed class DashSystemController : IDashSystemController
    {
        public event UserBalanceNotification UserBalanceWarning;

        private List<IUser> Users { get; } = new List<IUser>();
        private List<IProduct> Products { get; } = new List<IProduct>();
        private List<ITransaction> Transactions { get; } = new List<ITransaction>();

        public IEnumerable<IProduct> ActiveProducts => Products.Where(p => p.IsActive);

        private readonly string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "CsvData");
        
        public DashSystemController()
        {
            Console.WriteLine(dataDirectory);
            LoadData();
            SubscribeEvents();
        }

        public ITransaction BuyProduct(IUser user, IProduct product)
        {
            BuyTransaction buyTransaction = new BuyTransaction(user, product, DateTime.Now);
            
            return ExecuteTransaction(buyTransaction);
        }

        public ITransaction AddCreditsToAccount(IUser user, decimal amount)
        {
            InsertCashTransaction insertCashTransaction = new InsertCashTransaction(user, DateTime.Now, amount); 
            
            return ExecuteTransaction(insertCashTransaction);
        }

        public IProduct GetProductById(uint productId)
        {
            IProduct product = Products.Find(p => p.Id == productId);

            if (product == null)
            {
                throw new ProductNotFoundException(productId); 
            }

            return product;
        }

        public IEnumerable<IUser> GetUsers(Func<IUser, bool> predicate)
        {
            IEnumerable<IUser> users = Users.FindAll(u => predicate(u));

            return users;
        }

        public IUser GetUserByUsername(string username)
        {
            IUser user = Users.Find(u => u.Username == username);

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            return user;
        }

        public IEnumerable<ITransaction> GetTransactions(IUser user, int count)
        {
            return Transactions
                .Where(t => t.User.Equals(user))
                .OrderBy(t => t.Date)
                .Take(count);
        }

        private ITransaction ExecuteTransaction(ITransaction transaction)
        {
            transaction.Execute();
            transaction.Log(dataDirectory);
            Transactions.Add(transaction);

            return transaction;
        }

        private void LoadData()
        {
            string usersCsvPath = Path.Combine(dataDirectory, Constants.UsersCsvFileName);
            string productsCsvPath = Path.Combine(dataDirectory, Constants.ProductsCsvFileName);
            string buyTransactionsCsvPath = Path.Combine(dataDirectory, Constants.BuyTransactionsFileName);
            string insertCashTransactionCsvPath = Path.Combine(dataDirectory, Constants.InsertCashTransactionsFileName);

            LoadFromCsvFile(new CsvDataReader<UserCsvData>(','), usersCsvPath, 
                csvData => Users.Add((User)csvData));
            
            LoadFromCsvFile(new CsvDataReader<ProductCsvData>(';'), productsCsvPath, 
                csvData => Products.Add((Product)csvData));
            
            LoadFromCsvFile(new CsvDataReader<BuyTransactionCsvData>(';'), buyTransactionsCsvPath, 
                csvData => Transactions.Add(csvData.ToTransaction(this)));
            
            LoadFromCsvFile(new CsvDataReader<InsertCashTransactionCsvData>(';'), insertCashTransactionCsvPath, 
                csvData => Transactions.Add(csvData.ToTransaction(this)));
        }

        private void SubscribeEvents()
        {
            foreach (IUser user in Users)
            {
                user.LowFundsWarning += (amount) => UserBalanceWarning?.Invoke(user, amount);
            }
        }
        
        private static void LoadFromCsvFile<T>(CsvDataReader<T> dataReader, string path, Action<T> callbackForEachItem)
            where T : ICsvData, new()
        {
            IEnumerable<T> csvData = dataReader.ReadFile(path);

            foreach (T item in csvData)
            {
                callbackForEachItem?.Invoke(item);
            }
        }
    }
}