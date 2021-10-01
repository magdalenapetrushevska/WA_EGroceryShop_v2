using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Repository.Interface
{
    public interface IProductRepository
    {
        public Product GetProductWithReviews(Guid id);
    }
}
