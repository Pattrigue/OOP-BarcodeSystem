using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BarcodeSystem.CsvDataAccess;
using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.Core
{
    public sealed class BarcodeSystemManager : IBarcodeSystemManager
    {
        public event UserBalanceNotification UserBalanceWarning;

        private List<IUser> Users { get; } = new List<IUser>();
        private List<IProduct> Products { get; } = new List<IProduct>();
        private List<ITransaction> Transactions { get; } = new List<ITransaction>();

        public IEnumerable<IProduct> ActiveProducts => Products.Where(p => p.IsActive);

        private bool loggingEnabled;
        
        private readonly string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "CsvData");
        
        public BarcodeSystemManager()
        {
            LoadCsvData();
            ExecuteLoggedTransactions();
            SubscribeEvents();
        }

        public BarcodeSystemManager EnableLogging()
        {
            loggingEnabled = true;

            return this;
        }

        public BuyTransaction BuyProduct(IUser user, IProduct product, uint count = 1)
        {
            BuyTransaction buyTransaction = new BuyTransaction(user, product, DateTime.Now, count);
            
            ExecuteTransaction(buyTransaction);

            return buyTransaction;
        }

        public InsertCashTransaction AddCreditsToAccount(IUser user, decimal amount)
        {
            InsertCashTransaction insertCashTransaction = new InsertCashTransaction(user, DateTime.Now, amount); 
            
            ExecuteTransaction(insertCashTransaction);

            return insertCashTransaction;
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

        private void ExecuteTransaction(ITransaction transaction)
        {
            transaction.Execute();

            if (loggingEnabled)
            {
                transaction.Log(dataDirectory);
            }

            Transactions.Add(transaction);
        }

        private void LoadCsvData()
        {
            string usersCsvPath = Path.Combine(dataDirectory, Constants.UsersCsvFileName);
            string productsCsvPath = Path.Combine(dataDirectory, Constants.ProductsCsvFileName);
            string buyTransactionsCsvPath = Path.Combine(dataDirectory, Constants.BuyTransactionsFileName);
            string insertCashTransactionCsvPath = Path.Combine(dataDirectory, Constants.InsertCashTransactionsFileName);

            LoadFromCsvFile(new CsvDataReader<UserCsvData>(','), usersCsvPath, 
                csvData => Users.Add((User)csvData));
            
            LoadFromCsvFile(new CsvDataReader<ProductCsvData>(';'), productsCsvPath, 
                csvData => Products.Add((Product)csvData));
            
            LoadFromCsvFile(new CsvDataReader<InsertCashTransactionCsvData>(';'), insertCashTransactionCsvPath, 
                csvData => Transactions.Add(csvData.ToTransaction(this)));
            
            LoadFromCsvFile(new CsvDataReader<BuyTransactionCsvData>(';'), buyTransactionsCsvPath, 
                csvData => Transactions.Add(csvData.ToTransaction(this)));
        }

        private void ExecuteLoggedTransactions()
        {
            foreach (ITransaction transaction in Transactions)
            {
                transaction.Execute();
            }
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

            if (callbackForEachItem != null)
            {
                foreach (T item in csvData)
                {
                    callbackForEachItem.Invoke(item);
                }
            }
        }
    }
}