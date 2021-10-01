using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Repository.Interface
{
    public interface IProductPromotionRepository
    {
        public ProductPromotion GetPromotionWithProduct(Guid id);
    }
}
