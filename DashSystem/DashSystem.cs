using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DashSystem.Products;
using DashSystem.Transactions;
using DashSystem.Users;

namespace DashSystem
{
    public delegate void UserBalanceNotification(IUser user, decimal balance);

    public sealed class DashSystem : IDashSystem
    {
        public event UserBalanceNotification UserBalanceWarning;
        
        public List<IUser> Users { get; }
        public List<IProduct> Products { get; }
        public List<ITransaction> Transactions { get; }

        public IEnumerable<IProduct> ActiveProducts => Products.Where(p => p.IsActive);

        public class CSVData
        {
            public CSVData(string csvLine, string separator)
            {
                string[] fields = csvLine.Split(separator);
            }
        }
        
        public DashSystem()
        {
            string path = @"D:\Jottacloud\AAU 3. semester\OOP\Eksamensopgave 1\Eksamensopgave - Copy\Stregsystem\Data\products.csv";

            Console.WriteLine(path);
            
            List<CSVData> loadedCsvData = File.ReadAllLines(path)
                .Skip(1)
                .Select(csvLine => new CSVData(csvLine, ";"))
                .ToList();
            
            foreach (CSVData csvData in loadedCsvData)
            {
                Console.WriteLine();
            }
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
            Transactions.Add(transaction);

            return transaction;
        }
    }
}