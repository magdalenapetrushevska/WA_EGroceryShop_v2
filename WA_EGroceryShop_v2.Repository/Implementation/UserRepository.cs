using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;
using WA_EGroceryShop_v2.Repository.Interface;


namespace WA_EGroceryShop_v2.Repository.Implementation
{
    public class UserRepository  : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ApplicationUser>();
        }
        public IEnumerable<ApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public ApplicationUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.ProductInShoppingCarts")
               .Include("UserCart.ProductInShoppingCarts.Product")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(ApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
