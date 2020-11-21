using System;
using DashSystem.Products;

namespace DashSystem.Products
{
    public class Product : IProduct
    {
        private const uint MinId = 1;

        private static uint idCounter;
        
        private uint id;
        
        private string name;
        
        public Product(string name, decimal price, bool isActive, bool canBeBoughtOnCredit)
        {
            Id = idCounter++;
            Name = name;
            Price = price;
            IsActive = isActive;
            CanBeBoughtOnCredit = canBeBoughtOnCredit;
        }
        
        public uint Id
        {
            get => id;
            set
            {
                if (value < MinId)
                {
                    throw new ArgumentException($"ID must not be less than {MinId}!");
                }

                id = value;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be null or empty!");
                }

                name = value;
            }
        }
        
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public bool CanBeBoughtOnCredit { get; set; }

        public override string ToString() => $"{Id} {Name} {Price}";
    }
}