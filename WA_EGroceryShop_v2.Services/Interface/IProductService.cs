using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.DTO;
using WA_EGroceryShop_v2.Domain.Enums;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Product p);
        void UpdeteExistingProduct(Product p);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
        List<CategoryEnum> GetListOfCategories();
        List<SubcategoryEnum> GetListOfSubcategories();
        List<Product> GetProductsByCategory(string category);
        List<Product> GetProductsWithName(string searchString);
        List<SubcategoryEnum> GetSubcategoriesOfCategory(string category);
        List<Product> GetProductsBySubcategory(string subcategory);
        public Product GetProductWithReviews(Guid id);
    }
}
