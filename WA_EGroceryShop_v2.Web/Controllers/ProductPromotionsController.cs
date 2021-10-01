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
using WA_EGroceryShop_v2.Repository;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Web.Controllers
{
    public class ProductPromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductPromotionService _productPromotionService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductPromotionsController(ApplicationDbContext context, IProductPromotionService productPromotionService, IProductService productService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _productPromotionService = productPromotionService;
            _productService = productService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: ProductPromotions
        public IActionResult Index()
        {
            
            _productPromotionService.CheckPromotionEndDates();

            var promotionsWithoutProducts = _productPromotionService.GetAllProductPromotions();
            List<ProductPromotion> promotionsWithProducts = new List<ProductPromotion>();
            foreach(var promo in promotionsWithoutProducts)
            {
                var promoWithProduct = _productPromotionService.GetPromotionWithProduct(promo.Id);
                promotionsWithProducts.Add(promoWithProduct);
            }


            return View(promotionsWithProducts);
        }

        // GET: ProductPromotions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPromotion = await _context.ProductPromotions
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productPromotion == null)
            {
                return NotFound();
            }

            return View(productPromotion);
        }

        // GET: ProductPromotions/Create
        public IActionResult Create(Guid id)
        {
            var product = _productService.GetDetailsForProduct(id);
            if (product != null)
            {
                if (product.HasDiscount)
                {
                    return RedirectToAction("Index", "Products");
                }
            }

            ViewBag.ProductId = id;
            return View();
        }

        // POST: ProductPromotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,DiscountPercentage,EndDateOfPromotion,ImageFile")] ProductPromotion productPromotion, Guid id)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(productPromotion.ImageFile.FileName);
                string extension = Path.GetExtension(productPromotion.ImageFile.FileName);
                productPromotion.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/PromotionImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    productPromotion.ImageFile.CopyToAsync(fileStream);
                }


                productPromotion.ProductId = id;
                var product = _productService.GetDetailsForProduct(id);


                //treba da presmetame popust i da ja dodelime discountprice na soodvetniot produkt
                int oldPrice = product.Price;
                int newPrice = oldPrice - (int)(oldPrice * productPromotion.DiscountPercentage / 100);
                product.HasDiscount = true;
                product.DiscountPrice = newPrice;

                productPromotion.Product = product;

                this._productPromotionService.CreateNewProductPromotion(productPromotion);
                this._productService.UpdeteExistingProduct(product);
              
               
                return RedirectToAction(nameof(Index));
            }
            
            return View(productPromotion);
        }

        // GET: ProductPromotions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPromotion = await _context.ProductPromotions.FindAsync(id);
            if (productPromotion == null)
            {
                return NotFound();
            }
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productPromotion.ProductId);
            return View(productPromotion);
        }

        // POST: ProductPromotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,ProductId,DiscountPercentage,EndDateOfPromotion,ImageFile,Id")] ProductPromotion productPromotion)
        {
            if (id != productPromotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPromotionExists(productPromotion.Id))
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
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", productPromotion.ProductId);
            return View(productPromotion);
        }

        // GET: ProductPromotions/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

          
            var productPromotion = this._productPromotionService.GetDetailsForProductPromotion(id);

            if (productPromotion == null)
            {
                return NotFound();
            }

            return View(productPromotion);
        }

        // POST: ProductPromotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
                
            var productPromotion = this._productPromotionService.GetDetailsForProductPromotion(id);

            //za update na podatoci za produktot
            var product = _productService.GetDetailsForProduct(productPromotion.ProductId);

            product.DiscountPrice = 0;
            product.HasDiscount = false;
            

            this._productPromotionService.DeleteProductPromotion(id);
            this._productService.UpdeteExistingProduct(product);
            return RedirectToAction(nameof(Index));

        }

        private bool ProductPromotionExists(Guid id)
        {
            return _context.ProductPromotions.Any(e => e.Id == id);
        }
    }
}
