using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class ProductPromotion : BaseEntity
    {
        public string Title { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime EndDateOfPromotion { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
