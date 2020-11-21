using System;

namespace DashSystem.Products
{
    public sealed class SeasonalProduct : Product
    {
        public DateTime SeasonStartDate { get; }
        public DateTime SeasonEndDate { get; }
        
        public SeasonalProduct(string name, decimal price, bool isActive, bool canBeBoughtOnCredit, 
            DateTime seasonStartDate, DateTime seasonEndDate)
            : base(name, price, isActive, canBeBoughtOnCredit)
        {
            SeasonStartDate = seasonStartDate;
            SeasonEndDate = seasonEndDate;
        }
    }
}