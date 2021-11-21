using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GG.Core
{
    public class CartItem<T> :IEntity<string>, IEquatable<CartItem<T>> where T :  IRemarkableItem
    {

        public CartItem()
        {

        }


        public CartItem(T Item)
        {
            this.Item = Item;
        }

       
        public int Quantity { get; set; } = 1;

       

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public virtual T Item { get; set; }


       
        public int ShoppingCartId { get; set; }

        public virtual ShoppingCart<T> ShoppingCart { get; set; }

        [NotMapped]
        public string Description => this.Item.Review;

        [NotMapped]
        public decimal UnitPrice => this.Item.Price;

        [NotMapped]
        public decimal Total => UnitPrice * Quantity;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();

        public bool Equals(CartItem<T> other)
        {

            if (other != null)
            {
                return this.Item.Guid == other.Item.Guid;
            }
            return false;
            
        }
    }
}
