using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WA_EGroceryShop_v2.Domain.Enums
{
    public enum CategoryEnum
    {
        [Display(Name = "Fruit and Vegetables")]
        Fruit_And_Vegetables,
        Snacks,
        Beverages,
        [Display(Name = "Beauty and Personal care")]
        Beauty_And_Personal_Care,
        [Display(Name = "Meat and Seefood")]
        Meat_And_SeeFood,
        [Display(Name = "Diary, Eggs and Cheese")]
        Diary_Eggs_And_Cheese,
        [Display(Name = "Breads and Bakery")]
        Breads_And_Bakery,
        [Display(Name = "Deli and Prepared food")]
        Deli_And_Prepared_Food,
        [Display(Name = "Frozen food")]
        Frozen_food,
        [Display(Name = "Pantry Staples")]
        Pantry_Staples,
        [Display(Name = "Baby Food and Care")]
        Baby_Food_And_Care,
        [Display(Name = "Home and Kitchen")]
        Home_And_Kitchen
    };
}
