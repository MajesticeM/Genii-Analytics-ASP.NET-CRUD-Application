using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Genii_Assessment.Models
{
    /// <summary>
    /// Represents a product that can be stored in inventory and added to invoices.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key of the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        [Required]
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        /// <summary>
        /// Cost per individual item.
        /// </summary>
        [Required]
        [Range(0.01, 999999999)]
        public decimal CostPerItem { get; set; }

        /// <summary>
        /// Current stock level.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        /// <summary>
        /// Minimum stock level before restocking is required.
        /// </summary>
        [Required]
        public int RestockThreshold { get; set; }

        /// <summary>
        /// Indicates if the product is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date the product was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Navigation property for invoice items referencing this product.
        /// </summary>
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        /// <summary>
        /// Constructor initializes default values.
        /// </summary>
        public Product()
        {
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            InvoiceItems = new HashSet<InvoiceItem>();
        }
    }
}