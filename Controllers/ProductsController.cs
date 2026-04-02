using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Genii_Assessment.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

namespace Genii_Assessment.Controllers
{
    /// Handles product management.
    /// Users with Admin, User, or Manager roles may access this controller.
    [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProductsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        /// <summary>
        /// Displays a list of active products.
        /// </summary>
        public async Task<ActionResult> Index()
        {
            var products = await _dbContext.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        /// <summary>
        /// Displays product details.
        /// </summary>
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _dbContext.Products.FindAsync(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Displays the create product form.
        /// </summary>
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the edit form for an existing product.
        /// </summary>
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _dbContext.Products.FindAsync(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existingProduct = await _dbContext.Products.FindAsync(product.Id);

            if (existingProduct == null)
            {
                return HttpNotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.CostPerItem = product.CostPerItem;
            existingProduct.QuantityInStock = product.QuantityInStock;
            existingProduct.RestockThreshold = product.RestockThreshold;
            existingProduct.IsActive = product.IsActive;

            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the deactivate confirmation view.
        /// </summary>
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _dbContext.Products.FindAsync(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Soft deletes a product by marking it inactive.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.IsActive = false;

            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product deactivated successfully.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}