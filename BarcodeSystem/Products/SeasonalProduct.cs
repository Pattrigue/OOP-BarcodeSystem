using System;

namespace BarcodeSystem.Products
{
    public sealed class SeasonalProduct : Product
    {
        public DateTime SeasonStartDate { get; }
        public DateTime SeasonEndDate { get; }
        
        public SeasonalProduct(uint id, string name, decimal price, bool isActive, bool canBeBoughtOnCredit, 
            DateTime seasonStartDate, DateTime seasonEndDate)
            : base(id, name, price, isActive, canBeBoughtOnCredit)
        {
            SeasonStartDate = seasonStartDate;
            SeasonEndDate = seasonEndDate;
        }
    }
}