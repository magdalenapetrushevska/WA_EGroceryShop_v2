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
    public class ProductPromotionService : IProductPromotionService
    {
        private readonly IRepository<ProductPromotion> _productPromotionRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductPromotionRepository _productPromotionRepo;

        public ProductPromotionService(IRepository<ProductPromotion> productPromotionRepository, IRepository<Product> productRepository, IWebHostEnvironment hostEnvironment, IProductPromotionRepository _productPromotionRepo)
            {
            _productPromotionRepository = productPromotionRepository;
            _productRepository = productRepository;
            _hostEnvironment = hostEnvironment;
            this._productPromotionRepo = _productPromotionRepo;
        }


        public void CreateNewProductPromotion(ProductPromotion p)
        {
 

            this._productPromotionRepository.Insert(p);
        }

        public void DeleteProductPromotion(Guid id)
        {
            var productPromotion = this.GetDetailsForProductPromotion(id);

            //brishemo gu sliku za promociju 
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/PromotionImages", productPromotion.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            this._productPromotionRepository.Delete(productPromotion);
        }

        public List<ProductPromotion> GetAllProductPromotions()
        {
            return this._productPromotionRepository.GetAll().ToList();
        }

        public ProductPromotion GetDetailsForProductPromotion(Guid? id)
        {
            return this._productPromotionRepository.Get(id);
        }
        public void UpdeteExistingProductPromotion(ProductPromotion p)
        {
            this._productPromotionRepository.Update(p);
        }


        public void CheckPromotionEndDates()
        {
            

            List<ProductPromotion> promotions = new List<ProductPromotion>();
               promotions =  _productPromotionRepository.GetAll().ToList();

            for (int i = 0; i < promotions.Count; i++)
            {
                if (DateTime.Now >= promotions[i].EndDateOfPromotion)
                {

                    var productToBeUpdated = _productRepository.Get(promotions[i].ProductId);

                    productToBeUpdated.DiscountPrice = 0;
                    productToBeUpdated.HasDiscount = false;

                    //brishemo gu sliku za promociju 
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/PromotionImages", promotions[i].ImageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }



                    DeleteProductPromotion(promotions[i].Id);
                    _productRepository.Update(productToBeUpdated);
                }
               
            }


        }


        public ProductPromotion GetPromotionWithProduct(Guid id)
        {
            return _productPromotionRepo.GetPromotionWithProduct(id);
        }







    }
}
