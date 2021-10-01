using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public virtual DbSet<ProductPromotion> ProductPromotions { get; set; }
        public virtual DbSet<ProductCategoryPromotion> ProductCategoryPromotions { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();



            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ProductId);


            builder.Entity<ShoppingCart>()
                .HasOne<ApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);



            builder.Entity<ProductInOrder>()
                .HasOne(z => z.OrderedProduct)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ApplicationUser>()
               .HasMany(c => c.Favorites)
               .WithOne(c => c.User);

            builder.Entity<Product>()
               .HasMany(m => m.Reviews)
               .WithOne(m => m.Product);



        }




    }
}
