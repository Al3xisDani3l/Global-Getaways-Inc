using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GG.Core
{
    public class ShoppingCart
    {
        public List<PackageToCart<IRemarkable>> PackageList { get; private set; }

        public decimal Subtotal
        {
            get
            {
               return PackageList.Sum(p => p.Total);
            }
        }

        public ShoppingCart()
        {
            PackageList = new List<PackageToCart<IRemarkable>>();
        }

        public void Add(IRemarkable product)
        {

            var packageToCart = new PackageToCart<IRemarkable>(product);

            var result = PackageList.FirstOrDefault(p => p.Equals(packageToCart));

            if (result != null)
            {
                result.Quantity++;
                return;
            }
            PackageList.Add(packageToCart);

        }

        public void Remove(IRemarkable product)
        {
            

            var result = PackageList.FirstOrDefault(p => p.Product.Guid == product.Guid);

            if (result != null)
            {
                if (result.Quantity > 1)
                {
                    result.Quantity--;
                    return;
                }

                PackageList.Remove(result);
            }
        }

        public void RemoveAll(IRemarkable product)
        {
            var result = PackageList.FirstOrDefault(p => p.Product.Guid == product.Guid);

            if (result != null) PackageList.Remove(result);
        }

    }
}
