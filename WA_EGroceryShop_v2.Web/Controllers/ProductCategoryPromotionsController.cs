using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.Enums;
using WA_EGroceryShop_v2.Repository;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Web.Controllers
{
    public class ProductCategoryPromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductCategoryPromotionService _productCategoryPromotionService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductCategoryPromotionsController(ApplicationDbContext context, IProductCategoryPromotionService productCategoryPromotionService, IProductService productService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _productCategoryPromotionService = productCategoryPromotionService;
            _productService = productService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: ProductCategoryPromotions
        public IActionResult Index()
        {
            _productCategoryPromotionService.CheckPromotionEndDates();

            return View(_productCategoryPromotionService.GetAllProductCategoryPromotions());
        }

        // GET: ProductCategoryPromotions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategoryPromotion = await _context.ProductCategoryPromotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategoryPromotion == null)
            {
                return NotFound();
            }

            return View(productCategoryPromotion);
        }

        // GET: ProductCategoryPromotions/Create
        public IActionResult Create()
        {
            ViewBag.ListOfCategories = Enum.GetValues(typeof(CategoryEnum));
            return View();
        }

        // POST: ProductCategoryPromotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Category,DiscountPercentage,EndDateOfPromotion,ImageFile,Id")] ProductCategoryPromotion productCategoryPromotion)
        {
            if (ModelState.IsValid)
            {

                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(productCategoryPromotion.ImageFile.FileName);
                string extension = Path.GetExtension(productCategoryPromotion.ImageFile.FileName);
                productCategoryPromotion.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/PromotionImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    productCategoryPromotion.ImageFile.CopyToAsync(fileStream);
                }

                this._productCategoryPromotionService.CreateNewProductCategoryPromotion(productCategoryPromotion);
                List<Product> products = new List<Product>();
                products = _productService.GetProductsByCategory(productCategoryPromotion.Category.ToString());
                //products = _context.Products.Where(m => m.Category.Equals(productCategoryPromotion.Category));
               /* foreach (var product in products)
                {
                    //treba da presmetame popust i da ja dodelime discountprice na soodvetniot produkt
                    int oldPrice = product.Price;
                    int newPrice = oldPrice - (int)(oldPrice * productCategoryPromotion.DiscountPercentage / 100);
                    product.HasDiscount = true;
                    product.DiscountPrice = newPrice;

                    //_context.Update(product);
                    _productService.UpdeteExistingProduct(product);
                }*/



                for(int i = 0; i < products.Count; i++)
                {
                    int oldPrice = products[i].Price;
                    int newPrice = oldPrice - (int)(oldPrice * productCategoryPromotion.DiscountPercentage / 100);
                    products[i].HasDiscount = true;
                    products[i].DiscountPrice = newPrice;

                    
                    _productService.UpdeteExistingProduct(products[i]);
                }



                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCategoryPromotion);
        }

        // GET: ProductCategoryPromotions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategoryPromotion = await _context.ProductCategoryPromotions.FindAsync(id);
            if (productCategoryPromotion == null)
            {
                return NotFound();
            }
            return View(productCategoryPromotion);
        }

        // POST: ProductCategoryPromotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Category,DiscountPercentage,EndDateOfPromotion,ImageFile,Id")] ProductCategoryPromotion productCategoryPromotion)
        {
            if (id != productCategoryPromotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategoryPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryPromotionExists(productCategoryPromotion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productCategoryPromotion);
        }

        // GET: ProductCategoryPromotions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategoryPromotion = await _context.ProductCategoryPromotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategoryPromotion == null)
            {
                return NotFound();
            }

            return View(productCategoryPromotion);
        }

        // POST: ProductCategoryPromotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productCategoryPromotion = await _context.ProductCategoryPromotions.FindAsync(id);
            _context.ProductCategoryPromotions.Remove(productCategoryPromotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryPromotionExists(Guid id)
        {
            return _context.ProductCategoryPromotions.Any(e => e.Id == id);
        }
    }
}
