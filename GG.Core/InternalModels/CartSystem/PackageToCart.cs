using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{
    public class PackageToCart<T> : IEquatable<PackageToCart<T>> where T :  IRemarkable
    {

        public PackageToCart(T package)
        {
            this.Product = package;
        }

        public int Quantity { get; set; } = 1;

        public T Product { get; set; }

        public string Description => this.Product.Review;

        public decimal UnitPrice => this.Product.Price;

        public decimal Total => UnitPrice * Quantity;

        public bool Equals(PackageToCart<T> other)
        {

            if (other != null)
            {
                return this.Product.Guid == other.Product.Guid;
            }
            return false;
            
        }
    }
}
