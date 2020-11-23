using System;
using BarcodeSystem.Core;
using BarcodeSystem.Transactions;

namespace BarcodeSystem.CsvDataAccess
{
    public abstract class TransactionCsvData : ICsvData
    {
        protected uint Id;
        
        protected string Username;
        
        protected DateTime Date;
        
        protected decimal Amount;
        
        public abstract void ReadLine(char separator, string csvLine);

        public abstract ITransaction ToTransaction(IBarcodeSystemManager barcodeSystemManager);
    }
}