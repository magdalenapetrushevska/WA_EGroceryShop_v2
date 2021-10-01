using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WA_EGroceryShop_v2.Domain.Enums;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public CategoryEnum Category { get; set; }
        public SubcategoryEnum Subcategory { get; set; }
        [Required]
        public int Price { get; set; }
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
        public string Origin { get; set; }

        public bool HasDiscount { get; set; }
        public int DiscountPrice { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public IEnumerable<ProductInOrder> ProductInOrders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
