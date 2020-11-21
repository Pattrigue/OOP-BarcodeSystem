using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        
        public List<IUser> Users { get; } = new List<IUser>();
        public List<IProduct> Products { get; } = new List<IProduct>();
        public List<ITransaction> Transactions { get; } = new List<ITransaction>();

        public IEnumerable<IProduct> ActiveProducts => Products.Where(p => p.IsActive);

        public DashSystem()
        {
            LoadProductsFromCsvFile();
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

        private void LoadProductsFromCsvFile()
        {
            CsvDataReader<ProductCsvData> productCsvDataReader = new CsvDataReader<ProductCsvData>(';');
            IEnumerable<ProductCsvData> productCsvData = productCsvDataReader.ReadFile("products.csv");
            
            foreach (ProductCsvData productData in productCsvData)
            {
                Product product = new Product(productData.Id, productData.Name, productData.Price, productData.Active, false);
                
                Products.Add(product);
                Console.WriteLine($"Loaded product: {product}!");
            }
        }
        
        private ITransaction ExecuteTransaction(ITransaction transaction)
        {
            transaction.Execute();
            Transactions.Add(transaction);

            return transaction;
        }
    }
}