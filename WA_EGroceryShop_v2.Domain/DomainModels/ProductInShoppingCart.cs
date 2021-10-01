using System;
using System.Collections.Generic;
using System.Text;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class ProductInShoppingCart : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public int Quantity { get; set; }
    }
}
