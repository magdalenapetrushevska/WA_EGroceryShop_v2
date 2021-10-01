using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Repository.Interface;

namespace WA_EGroceryShop_v2.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Product> entities;

        string errorMessage = string.Empty;

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Product>();

        }
        
        public Product GetProductWithReviews(Guid id)
        {
            return entities
               .Include(z => z.Reviews)
               .ThenInclude(z =>z.User)
               .SingleOrDefaultAsync(z => z.Id == id).Result;
        }
    }
}
