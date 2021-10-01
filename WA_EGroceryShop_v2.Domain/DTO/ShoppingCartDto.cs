using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCart> Products { get; set; }
        public double TotalPrice { get; set; }
    }
}
