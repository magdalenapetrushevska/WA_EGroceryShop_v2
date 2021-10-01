using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Repository;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ReviewsController(ApplicationDbContext context, IProductService _productService, IWebHostEnvironment _hostEnvironment)
        {
            _context = context;
            this._productService = _productService;
            this._hostEnvironment = _hostEnvironment;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reviews.Include(r => r.Product).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [Authorize]
        // GET: Reviews/Create
        public IActionResult Create(Guid id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        [Authorize]
        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment,ImageFile,RatingGrade")] Review review, Guid id)
        {
            if (ModelState.IsValid)
            {

                // Save image to wwwroot / image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(review.ImageFile.FileName);
                string extension = Path.GetExtension(review.ImageFile.FileName);
                review.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/ReviewImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await review.ImageFile.CopyToAsync(fileStream);
                }

                review.ProductId = id;
                var product = _productService.GetDetailsForProduct(id);

                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                review.UserId = loggedInUserId;
                review.Posted = DateTime.Now;
                review.User = loggedInUser;

                review.Id = Guid.NewGuid();
                _context.Add(review);
                await _context.SaveChangesAsync();

                
                return RedirectToAction("Details","Products", new { id });
            }
            
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", review.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,Comment,ImageName,RatingGrade,Posted,UserId,Id")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", review.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(Guid id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
