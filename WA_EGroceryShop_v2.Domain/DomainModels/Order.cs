using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public IEnumerable<ProductInOrder> ProductInOrders { get; set; }
    }
}
