﻿using System;

namespace BarcodeSystem.Products
{
    public class Product : IProduct
    {
        private const uint MinId = 1;

        private uint id;
        
        private string name;
        
        public Product(uint id, string name, decimal price, bool isActive, bool canBeBoughtOnCredit = true)
        {
            Id = id;
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
        
        public decimal Price { get; }

        public bool IsActive { get; set; }

        public bool CanBeBoughtOnCredit { get; set; }

        public override string ToString() => $"{Id} {Name} {Price}";
    }
}