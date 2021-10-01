using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WA_EGroceryShop_v2.Domain.Enums
{
    public enum SubcategoryEnum
    {
        [Display(Name = "Non-alcoholic drinks")]
        Non_alcoholic_drinks,
        [Display(Name = "Alcoholic drinks")]
        Alcoholic_drinks,
        [Display(Name = "Hot drinks")]
        Hot_drinks,
        [Display(Name = "Red meat")]
        Red_meat,
        [Display(Name = "White meat")]
        White_meat,
        Seefood,
        Fruit,
        Vegetables,
        [Display(Name = "Salty snacks")]
        Salty_snacks,
        [Display(Name = "Sweet snacks")]
        Sweet_snacks,
        Dairy,
        Eggs,
        Breads,
        [Display(Name = "Baked goods and sweet treats")]
        Baked_goods_and_sweet_treats,
        [Display(Name = "Prepared foods")]
        Prepared_foods,
        Salad,
        [Display(Name = "Ice cream")]
        Ice_cream,
        [Display(Name = "Frozen Pizzas")]
        Frozen_Pizzas,
        [Display(Name = "Frozen vegetables")]
        Frozen_Vegetables,
        [Display(Name = "Frozen meals and entrees")]
        Frozen_meals_and_entrees,
        [Display(Name = "Pasta, noodles and sauces")]
        Pasta_noodles_and_sauces,
        [Display(Name = "Baking and cooking")]
        Baking_and_cooking,
        [Display(Name = "Canned, jarred and packaged")]
        Canned_jarred_and_packaged,
        [Display(Name = "Soups,stocks and broths")]
        Soups_stocks_and_broths,
        [Display(Name = "Herbs and spices")]
        Herbs_and_spices,
        [Display(Name = "Baby food")]
        Baby_food,
        [Display(Name = "Baby care")]
        Baby_care,
        [Display(Name = "Cleaning supplies")]
        Cleaning_supplies,
        [Display(Name = "Paper and plastic")]
        Paper_and_plastic,
        Dishwashing,
        Laundry,
        [Display(Name = "Office supplies")]
        Office_supplies,
        [Display(Name = "Feminine care")]
        Feminine_care,
        [Display(Name = "Make up")]
        Make_up,
        Condoms,
        [Display(Name = "Shaving and hair removal")]
        Shaving_and_hair_removal,
        Bath,
        [Display(Name = "Skin care")]
        Skin_care,
        [Display(Name = "Oral care")]
        Oral_care,
        [Display(Name = "Hair care")]
        Hair_care
    };
}
