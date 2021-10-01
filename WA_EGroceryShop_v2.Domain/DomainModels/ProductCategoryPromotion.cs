using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WA_EGroceryShop_v2.Domain.Enums;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class ProductCategoryPromotion : BaseEntity
    {
        public string Title { get; set; }
        public CategoryEnum Category { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime EndDateOfPromotion { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
