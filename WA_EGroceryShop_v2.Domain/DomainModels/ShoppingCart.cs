using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
    }
}
