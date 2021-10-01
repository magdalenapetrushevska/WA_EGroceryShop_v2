using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IProductPromotionService
    {
        void DeleteProductPromotion(Guid id);
        List<ProductPromotion> GetAllProductPromotions();
        ProductPromotion GetDetailsForProductPromotion(Guid? id);
        void CreateNewProductPromotion(ProductPromotion p);
        void UpdeteExistingProductPromotion(ProductPromotion p);
        void CheckPromotionEndDates();
        public ProductPromotion GetPromotionWithProduct(Guid id);
    }
}
