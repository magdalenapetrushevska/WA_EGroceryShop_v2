using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Domain.DomainModels
{
    public class Review : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Comment { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Range(1, 5,
       ErrorMessage = "Value for rating must be between 1 and 5.")]
        public float RatingGrade{ get; set; }
        public DateTime Posted { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
