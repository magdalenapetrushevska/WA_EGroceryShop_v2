using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.DTO;
using WA_EGroceryShop_v2.Domain.Enums;
using WA_EGroceryShop_v2.Repository;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IProductPromotionService _productPromotionService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(ApplicationDbContext context, IProductService productServicet, IProductPromotionService productPromotionService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _productService = productServicet;
            _productPromotionService = productPromotionService;
            this._hostEnvironment = hostEnvironment;
        }

        

        [Authorize]
        public IActionResult AddProductToCart(Guid? id)
        {
            var model = this._productService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddProductToCart([Bind("ProductId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            var result = this._productService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index","ShoppingCart");
            }

            return View(item);
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetProductWithReviews(id);

            


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.ListOfCategories = _productService.GetListOfCategories();
            ViewBag.ListOfSubcategories = _productService.GetListOfSubcategories();
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageFile,Category,Subcategory,Price,BrandName,Origin,Rating,Id")] Product product)
        {
            if (ModelState.IsValid)
            {

                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                product.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/ProductImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }


                this._productService.CreateNewProduct(product);
                return RedirectToAction(nameof(ProductCategories));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }


            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/ProductImages", product.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            ViewBag.ListOfCategories = _productService.GetListOfCategories();
            ViewBag.ListOfSubcategories = _productService.GetListOfSubcategories();

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,ImageFile,Category,Subcategory,Price,BrandName,Origin,Rating,Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                //Save image to wwwroot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                product.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/ProductImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }


                try
                {
                    this._productService.UpdeteExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Products", new { id });

            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            this._productService.DeleteProduct(id);

            return RedirectToAction("ProductCategories");
        }

        private bool ProductExists(Guid id)
        {

            return this._productService.GetDetailsForProduct(id) != null;
        }

        public IActionResult GetProductsByCategory(string id)
        {
            ViewBag.CategoryName = id.Replace("_", " ");
            ViewBag.Subcategories = _productService.GetSubcategoriesOfCategory(id);
            var list = this._productService.GetProductsByCategory(id);
            return View(list);
        }

        public IActionResult ProductCategories()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchProduct(string id)
        {
            if (id != null && !id.Equals(""))
            {
                ViewBag.SearchWord = id;
                List<Product> list = new List<Product>();

                list = _productService.GetProductsWithName(id);

                return View(list);
            }
            else
            {
                return RedirectToAction("ProductCategories");
            }
        }



        public IActionResult GetProductsBySubcategory(string id)
        {
            ViewBag.SubcategoryName = id.Replace("_", " ");
            var list = this._productService.GetProductsBySubcategory(id);
            return View(list);
        }




        /*[Authorize]
        public async Task<IActionResult> AddToFavoriteProducts(Guid id)
        {
            Favorite newFavoriteProduct = new Favorite();

            var favProduct = _context.Products.Find(id);
            newFavoriteProduct.ProductId = id;
            newFavoriteProduct.Product = favProduct;

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);
            newFavoriteProduct.UserId = loggedInUserId;
            newFavoriteProduct.User = loggedInUser;



            var siteFavorites = _context.Favorites.Include(p=>p.Product);

            foreach (var fav in siteFavorites)
            {
                if ((fav.ProductId.Equals(newFavoriteProduct.ProductId) && fav.UserId.Equals(newFavoriteProduct.UserId)))
                {
                    return RedirectToAction("UserFavoriteProductsList");
                }
            }

            _context.Add(newFavoriteProduct);
            await _context.SaveChangesAsync();



            return RedirectToAction("UserFavoriteProductsList");
        }*/

        /*[Authorize]
        public IActionResult UserFavoriteProductsList()
        {


            List<Favorite> favoritesList = new List<Favorite>();

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);

            var siteFavorites = _context.Favorites.Include(m => m.Product).Include(m => m.User).ThenInclude(m => m.Favorites);

            foreach (var fav in siteFavorites)
            {
                if (fav.UserId.Equals(loggedInUserId))
                {
                    favoritesList.Add(fav);
                }
            }


            return View(favoritesList);
        }*/



        /*[Authorize]
        public async Task<IActionResult> DeleteFromFavorites(Guid id)
        {

            var toRemove = await _context.Favorites.First(id);
            var toRemovee = _context.Favorites.Find(id);


            _context.Favorites.Remove(toRemove);
            _context.Favorites.Remove(id);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(UserFavoriteProductsList));
        }*/






    }
}
