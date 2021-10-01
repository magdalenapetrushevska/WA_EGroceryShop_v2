using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DTO;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
