using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.DTO;
using WA_EGroceryShop_v2.Domain.Enums;
using WA_EGroceryShop_v2.Repository.Interface;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductRepository _productRepo;


        public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger, IRepository<ProductInShoppingCart> productInShoppingCartRepository, IUserRepository userRepository, IWebHostEnvironment hostEnvironment, IProductRepository _productRepo)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            this._productRepo = _productRepo;
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.ProductId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.ProductId);

                if (product != null)
                {
                    ProductInShoppingCart itemToAdd = new ProductInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Product = product,
                        ProductId = product.Id,
                        ShoppingCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    this._productInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Product was successfully added into ShoppingCart");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something was wrong. ProductId or UserShoppingCard may be unaveliable!");
            return false;
        }

        public void CreateNewProduct(Product p)
        { 

            this._productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = this.GetDetailsForProduct(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/ProductImages", product.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            this._productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called!");
            return this._productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return this._productRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedProduct = product,
                ProductId = product.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingProduct(Product p)
        {
            this._productRepository.Update(p);
        }


        public List<CategoryEnum> GetListOfCategories()
        {
            //convert from Array to List
            var result = Enum.GetValues(typeof(CategoryEnum));
            List<CategoryEnum> lst = result.OfType<CategoryEnum>().ToList();

            return lst;
        }
        public List<SubcategoryEnum> GetListOfSubcategories()
        {

            var result = Enum.GetValues(typeof(SubcategoryEnum));
            List<SubcategoryEnum> lst = result.OfType<SubcategoryEnum>().ToList();

            return lst;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            var products = this._productRepository.GetAll();
            List<Product> list = new List<Product>();
            foreach (var pr in products)
            {
                if (pr.Category.ToString().Equals(category))
                {
                    list.Add(pr);
                }
            }
            return list;
        }

        public List<Product> GetProductsWithName(string searchString)
        {
            
                List<Product> products = new List<Product>();
                products = this._productRepository.GetAll().Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
                return products;
            
        }

        public List<SubcategoryEnum> GetSubcategoriesOfCategory(string category)
        {
            List<SubcategoryEnum> subCatValues = new List<SubcategoryEnum>();

            switch (category)
            {
                case "Baby_Food_And_Care":
                    subCatValues.Add(SubcategoryEnum.Baby_care);
                    subCatValues.Add(SubcategoryEnum.Baby_food);
                    break;
                case "Beauty_And_Personal_Care":
                    subCatValues.Add(SubcategoryEnum.Skin_care);
                    subCatValues.Add(SubcategoryEnum.Oral_care);
                    subCatValues.Add(SubcategoryEnum.Hair_care);
                    subCatValues.Add(SubcategoryEnum.Shaving_and_hair_removal);
                    subCatValues.Add(SubcategoryEnum.Bath);
                    subCatValues.Add(SubcategoryEnum.Make_up);
                    subCatValues.Add(SubcategoryEnum.Condoms);
                    subCatValues.Add(SubcategoryEnum.Feminine_care);
                    break;
                case "Beverages":
                    subCatValues.Add(SubcategoryEnum.Non_alcoholic_drinks);
                    subCatValues.Add(SubcategoryEnum.Alcoholic_drinks);
                    subCatValues.Add(SubcategoryEnum.Hot_drinks);
                    break;
                case "Breads_And_Bakery":
                    subCatValues.Add(SubcategoryEnum.Breads);
                    subCatValues.Add(SubcategoryEnum.Baked_goods_and_sweet_treats);
                    break;
                case "Deli_And_Prepared_Food":
                    subCatValues.Add(SubcategoryEnum.Prepared_foods);
                    subCatValues.Add(SubcategoryEnum.Salad);
                    break;
                case "Diary_Eggs_And_Cheese":
                    subCatValues.Add(SubcategoryEnum.Eggs);
                    subCatValues.Add(SubcategoryEnum.Dairy);
                    break;
                case "Frozen_food":
                    subCatValues.Add(SubcategoryEnum.Ice_cream);
                    subCatValues.Add(SubcategoryEnum.Frozen_Pizzas);
                    subCatValues.Add(SubcategoryEnum.Frozen_Vegetables);
                    subCatValues.Add(SubcategoryEnum.Frozen_meals_and_entrees);
                    break;
                case "Fruit_And_Vegetables":
                    subCatValues.Add(SubcategoryEnum.Fruit);
                    subCatValues.Add(SubcategoryEnum.Vegetables);
                    break;
                case "Home_And_Kitchen":
                    subCatValues.Add(SubcategoryEnum.Cleaning_supplies);
                    subCatValues.Add(SubcategoryEnum.Paper_and_plastic);
                    subCatValues.Add(SubcategoryEnum.Dishwashing);
                    subCatValues.Add(SubcategoryEnum.Laundry);
                    subCatValues.Add(SubcategoryEnum.Office_supplies);
                    break;
                case "Meat_And_SeeFood":
                    subCatValues.Add(SubcategoryEnum.Red_meat);
                    subCatValues.Add(SubcategoryEnum.White_meat);
                    subCatValues.Add(SubcategoryEnum.Seefood);
                    break;
                case "Pantry_Staples":
                    subCatValues.Add(SubcategoryEnum.Pasta_noodles_and_sauces);
                    subCatValues.Add(SubcategoryEnum.Baking_and_cooking);
                    subCatValues.Add(SubcategoryEnum.Canned_jarred_and_packaged);
                    subCatValues.Add(SubcategoryEnum.Soups_stocks_and_broths);
                    subCatValues.Add(SubcategoryEnum.Herbs_and_spices);
                    break;
                case "Snacks":
                    subCatValues.Add(SubcategoryEnum.Salty_snacks);
                    subCatValues.Add(SubcategoryEnum.Sweet_snacks);
                    break;
            }

            return subCatValues;
        }


        public List<Product> GetProductsBySubcategory(string subcategory)
        {
            var products = this._productRepository.GetAll();
            List<Product> list = new List<Product>();
            foreach (var pr in products)
            {
                if (pr.Subcategory.ToString().Equals(subcategory))
                {
                    list.Add(pr);
                }
            }
            return list;
        }


        public Product GetProductWithReviews(Guid id)
        {
            return _productRepo.GetProductWithReviews(id);
        }




    }
}
