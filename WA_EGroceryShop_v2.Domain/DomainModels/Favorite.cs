using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class Favorite : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
