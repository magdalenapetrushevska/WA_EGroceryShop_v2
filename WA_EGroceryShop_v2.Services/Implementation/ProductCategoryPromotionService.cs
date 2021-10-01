using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Repository.Interface;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Services.Implementation
{
    public class ProductCategoryPromotionService : IProductCategoryPromotionService
    {
        private readonly IRepository<ProductCategoryPromotion> _productCategoryPromotionRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductService _productService;

        public ProductCategoryPromotionService(IRepository<ProductCategoryPromotion> productCategoryPromotionRepository, IRepository<Product> productRepository, IWebHostEnvironment hostEnvironment, IProductService productService)
        {
            _productCategoryPromotionRepository = productCategoryPromotionRepository;
            _productRepository = productRepository;
            _hostEnvironment = hostEnvironment;
            _productService = productService;
        }


        public void CreateNewProductCategoryPromotion(ProductCategoryPromotion p)
        {

            

            this._productCategoryPromotionRepository.Insert(p);
        }

        public void DeleteProductCategoryPromotion(Guid id)
        {
            var productCategoryPromotion = this.GetDetailsForProductCategoryPromotion(id);

            //brishemo gu sliku za promociju 
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/PromotionImages", productCategoryPromotion.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            this._productCategoryPromotionRepository.Delete(productCategoryPromotion);
        }

        public List<ProductCategoryPromotion> GetAllProductCategoryPromotions()
        {
            return this._productCategoryPromotionRepository.GetAll().ToList();
        }

        public ProductCategoryPromotion GetDetailsForProductCategoryPromotion(Guid? id)
        {
            return this._productCategoryPromotionRepository.Get(id);
        }
        public void UpdeteExistingProductCategoryPromotion(ProductCategoryPromotion p)
        {
            this._productCategoryPromotionRepository.Update(p);
        }


        public void CheckPromotionEndDates()
        {
            List<ProductCategoryPromotion> promotions = new List<ProductCategoryPromotion>();
            promotions = _productCategoryPromotionRepository.GetAll().ToList();

            for (int i = 0; i < promotions.Count; i++)
            {
                if (DateTime.Now >= promotions[i].EndDateOfPromotion)
                {

                    //ti treba lista od produkti koi ja imaat taa kategorija, i na site niv update da se naprai
                    List<Product> products = new List<Product>();
                    products = _productService.GetProductsByCategory(promotions[i].Category.ToString());

                    for(int j = 0; j < products.Count; j++)
                    {
                        var productToBeUpdated = _productService.GetDetailsForProduct(products[j].Id);

                        productToBeUpdated.DiscountPrice = 0;
                        productToBeUpdated.HasDiscount = false;

                        _productService.UpdeteExistingProduct(productToBeUpdated);
                    }

                   

                    //brishemo gu sliku za promociju 
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/PromotionImages", promotions[i].ImageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }



                    DeleteProductCategoryPromotion(promotions[i].Id);
                    
                }

            }


        }
    }
}
