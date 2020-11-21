using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DashSystem.Products;
using DashSystem.Transactions;
using DashSystem.Users;
using DashSystem.CsvDataAccess;

namespace DashSystem
{
    public delegate void UserBalanceNotification(IUser user, decimal balance);

    public sealed class DashSystem : IDashSystem
    {
        public event UserBalanceNotification UserBalanceWarning;

        private List<IUser> Users { get; } = new List<IUser>();
        private List<IProduct> Products { get; } = new List<IProduct>();
        private List<ITransaction> Transactions { get; } = new List<ITransaction>();

        public IEnumerable<IProduct> ActiveProducts => Products.Where(p => p.IsActive);

        private readonly string dataDirectory = Path.Combine(
            Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))!,
            "Data");
        
        public DashSystem()
        {
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
            Console.WriteLine("Loading users...");
            LoadFromCsvFile(new CsvDataReader<UserCsvData>(','), 
                Path.Combine(dataDirectory, Constants.UsersCsvFileName), 
                csvData => Users.Add((User)csvData));
            
            Console.WriteLine("Loading products...");
            LoadFromCsvFile(new CsvDataReader<ProductCsvData>(';'), 
                Path.Combine(dataDirectory, Constants.ProductsCsvFileName), 
                csvData => Products.Add((Product)csvData));
            
            Console.WriteLine("Loading buy transactions...");
            LoadFromCsvFile(new CsvDataReader<BuyTransactionCsvData>(';'), Path.Combine(dataDirectory, 
                    Constants.BuyTransactionsFileName),
                csvData => Transactions.Add(csvData.ToTransaction(this)));
            
            Console.WriteLine("Loading insert cash transactions...");
            LoadFromCsvFile(new CsvDataReader<InsertCashTransactionCsvData>(';'), Path.Combine(dataDirectory, 
                    Constants.InsertCashTransactionsFileName),
                csvData => Transactions.Add(csvData.ToTransaction(this)));

            Console.WriteLine("Successfully loaded all data!");
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