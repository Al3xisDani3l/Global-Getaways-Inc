using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG.Core
{
    public class ShoppingCart<TItem>:BaseEntity where TItem  : IRemarkableItem
    {
      
        public string IdUser { get; set; }

        public virtual PrivateUser User { get; private set; }

        public virtual ICollection<CartItem<TItem>> Items { get; private set; }

        [NotMapped]
        public decimal Subtotal
        {
            get
            {
               return Items.Sum(p => p.Total);
            }
        }

        public ShoppingCart(string idUser)
        {
            Items = new HashSet<CartItem<TItem>>();

            IdUser = idUser;
        }

        public ShoppingCart()
        {
            Items = new HashSet<CartItem<TItem>>();
        }

        public void Add(TItem product)
        {
            var packageToCart = new CartItem<TItem>(product);

            var result = Items.FirstOrDefault(p => p.Equals(packageToCart));

            if (result != null)
            {
                result.Quantity++;
                return;
            }
        
            Items.Add(packageToCart);

        }

        public void Remove(TItem product)
        {
            

            var result = Items.FirstOrDefault(p => p.Item.Guid == product.Guid);

            if (result != null)
            {
                if (result.Quantity > 1)
                {
                    result.Quantity--;
                    return;
                }

                Items.Remove(result);
            }
        }

        public void RemoveAll(TItem product)
        {
            var result = Items.FirstOrDefault(p => p.Item.Guid == product.Guid);

            if (result != null) Items.Remove(result);
        }

    }
}
