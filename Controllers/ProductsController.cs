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


        /// Default constructor.

        public ProductsController()
        {
            _dbContext = new ApplicationDbContext();
        }


        /// Displays a list of active products.

        /// <returns>Product list view.</returns>
        public async Task<ActionResult> Index()
        {
            var products = await _dbContext.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }


        /// Displays product details.

        /// <param name="id">Product identifier.</param>
        /// <returns>Details view.</returns>
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


        /// Displays the create product form.

        /// <returns>Create view.</returns>
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {
            return View();
        }


        /// Creates a new product.

        /// <param name="product">Product model.</param>
        /// <returns>Redirect to product list on success.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
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


        /// Displays the edit form for an existing product.

        /// <param name="id">Product identifier.</param>
        /// <returns>Edit view.</returns>
        [Authorize(Roles = "Admin,Manager")]
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


        /// Updates an existing product.

        /// <param name="product">Product model.</param>
        /// <returns>Redirect to product list on success.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
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


        /// Displays the deactivate confirmation view.

        /// <param name="id">Product identifier.</param>
        /// <returns>Delete confirmation view.</returns>
        [Authorize(Roles = "Admin,Manager")]
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


        /// Soft deletes a product by marking it inactive.

        /// <param name="id">Product identifier.</param>
        /// <returns>Redirect to product list on success.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
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


        /// Releases controller resources.

        /// <param name="disposing">Dispose flag.</param>
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