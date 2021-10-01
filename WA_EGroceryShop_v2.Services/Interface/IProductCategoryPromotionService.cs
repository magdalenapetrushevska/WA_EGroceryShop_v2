using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IProductCategoryPromotionService
    {
        void DeleteProductCategoryPromotion(Guid id);
        List<ProductCategoryPromotion> GetAllProductCategoryPromotions();
        ProductCategoryPromotion GetDetailsForProductCategoryPromotion(Guid? id);
        void CreateNewProductCategoryPromotion(ProductCategoryPromotion p);
        void UpdeteExistingProductCategoryPromotion(ProductCategoryPromotion p);
        void CheckPromotionEndDates();
    }
}
