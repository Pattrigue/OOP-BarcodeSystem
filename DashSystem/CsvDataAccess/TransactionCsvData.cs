using System;
using DashSystem.Core;
using DashSystem.Transactions;

namespace DashSystem.CsvDataAccess
{
    public abstract class TransactionCsvData : ICsvData
    {
        protected uint Id;
        
        protected string Username;
        
        protected DateTime Date;
        
        protected decimal Amount;
        
        public abstract void ReadLine(char separator, string csvLine);

        public abstract ITransaction ToTransaction(IDashSystemController dashSystemController);
    }
}