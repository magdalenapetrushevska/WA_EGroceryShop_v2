using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Repository.Interface;

namespace WA_EGroceryShop_v2.Repository.Implementation
{
    public class ProductPromotionRepository : IProductPromotionRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ProductPromotion> entities;

        string errorMessage = string.Empty;

        public ProductPromotionRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ProductPromotion>();

        }

        public ProductPromotion GetPromotionWithProduct(Guid id)
        {
            return entities
               .Include(z => z.Product)
               .SingleOrDefaultAsync(z => z.Id == id).Result;
        }
    }
}
