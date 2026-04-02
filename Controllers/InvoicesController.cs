using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Genii_Assessment.Models;
using Genii_Assessment.ViewModels;
using Microsoft.AspNet.Identity;

namespace Genii_Assessment.Controllers
{
     
    /// Handles invoice CRUD for authenticated users.
    /// Users may only access their own invoices in this feature.
     
    [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoicesController()
        {
            _dbContext = new ApplicationDbContext();
        }

         
        /// Displays invoices created by the currently logged-in user.
         
        public async Task<ActionResult> Index()
        {
            var currentUserId = User.Identity.GetUserId();

            var invoices = await _dbContext.Invoices
                .Include(i => i.InvoiceItems)
                .Where(i => i.CreatedByUserId == currentUserId)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            var model = invoices.Select(i => new InvoiceListItemViewModel
            {
                Id = i.Id,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                ItemCount = i.InvoiceItems.Count
            }).ToList();

            return View(model);
        }

         
        /// Displays details for a single user-owned invoice.
         
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentUserId = User.Identity.GetUserId();

            var invoice = await _dbContext.Invoices
                .Include(i => i.CreatedByUser)
                .Include(i => i.InvoiceItems.Select(ii => ii.Product))
                .FirstOrDefaultAsync(i => i.Id == id.Value && i.CreatedByUserId == currentUserId);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            return View(invoice);
        }


        /// Displays the create invoice form.

        public ActionResult Create()
        {
            var model = new InvoiceCreateEditViewModel();
            model.Items.Add(new InvoiceItemInputViewModel
            {
                Quantity = 1,
                UnitCost = 0.00m,
                LineTotal = 0.00m,
                ProductOptions = GetProductSelectListItems()
            });

            return View(model);
        }


        /// Creates a new invoice for the current user.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvoiceCreateEditViewModel model)
        {
            PopulateProductOptions(model);

            if (!ValidateInvoiceItems(model))
            {
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = User.Identity.GetUserId();

            var invoice = new Invoice
            {
                InvoiceDate = model.InvoiceDate,
                CreatedByUserId = currentUserId,
                TotalAmount = model.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                InvoiceItems = new List<InvoiceItem>()
            };

            foreach (var item in model.Items)
            {
                invoice.InvoiceItems.Add(new InvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    LineTotal = item.LineTotal
                });
            }

            _dbContext.Invoices.Add(invoice);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Invoice created successfully.";
            return RedirectToAction("Index");
        }

         
        /// Displays the edit form for a user-owned invoice.
         
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentUserId = User.Identity.GetUserId();

            var invoice = await _dbContext.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id.Value && i.CreatedByUserId == currentUserId);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            var model = new InvoiceCreateEditViewModel
            {
                Id = invoice.Id,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                Items = invoice.InvoiceItems.Select(ii => new InvoiceItemInputViewModel
                {
                    ProductId = ii.ProductId,
                    Quantity = ii.Quantity,
                    UnitCost = ii.UnitCost,
                    LineTotal = ii.LineTotal,
                    ProductOptions = GetProductSelectListItems()
                }).ToList()
            };

            if (!model.Items.Any())
            {
                model.Items.Add(BuildEmptyInvoiceItem());
            }

            return View(model);
        }

         
        /// Updates an existing invoice owned by the current user.
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(InvoiceCreateEditViewModel model)
        {
            PopulateProductOptions(model);

            if (!ValidateInvoiceItems(model))
            {
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = User.Identity.GetUserId();

            var invoice = await _dbContext.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == model.Id && i.CreatedByUserId == currentUserId);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            invoice.InvoiceDate = model.InvoiceDate;
            invoice.TotalAmount = model.TotalAmount;
            invoice.UpdatedAt = DateTime.UtcNow;

            _dbContext.InvoiceItems.RemoveRange(invoice.InvoiceItems);
            invoice.InvoiceItems.Clear();

            foreach (var item in model.Items)
            {
                invoice.InvoiceItems.Add(new InvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    LineTotal = item.LineTotal
                });
            }

            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Invoice updated successfully.";
            return RedirectToAction("Index");
        }

        private InvoiceItemInputViewModel BuildEmptyInvoiceItem()
        {
            return new InvoiceItemInputViewModel
            {
                Quantity = 1,
                UnitCost = 0.00m,
                LineTotal = 0.00m,
                ProductOptions = GetProductSelectListItems()
            };
        }

        private void PopulateProductOptions(InvoiceCreateEditViewModel model)
        {
            if (model.Items == null)
            {
                model.Items = new List<InvoiceItemInputViewModel>();
            }

            foreach (var item in model.Items)
            {
                item.ProductOptions = GetProductSelectListItems();
            }
        }

        private IEnumerable<SelectListItem> GetProductSelectListItems()
        {
            return _dbContext.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToList()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name + " (" + p.CostPerItem.ToString("0.00") + ")"
                });
        }

        private bool ValidateInvoiceItems(InvoiceCreateEditViewModel model)
        {
            if (model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "At least one invoice item is required.");
                return false;
            }

            foreach (var item in model.Items)
            {
                if (item.ProductId <= 0)
                {
                    ModelState.AddModelError("", "Each invoice item must have a selected product.");
                    return false;
                }

                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError("", "Each invoice item must have a quantity greater than zero.");
                    return false;
                }
            }

            return true;
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