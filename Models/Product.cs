using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Genii_Assessment.Models
{
    
    /// Represents a product that can be stored in inventory and added to invoices.
    
    public class Product
    {
        
        /// Primary key of the product.
        
        public int Id { get; set; }

        
        /// Name of the product.
        
        [Required]
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        
        /// Cost per individual item.
        
        [Required]
        [Range(0.01, 999999999)]
        public decimal CostPerItem { get; set; }

        
        /// Current stock level.
        
        [Required]
        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        
        /// Minimum stock level before restocking is required.
        
        [Required]
        public int RestockThreshold { get; set; }

        
        /// Indicates if the product is active.
        
        public bool IsActive { get; set; }

        
        /// Date the product was created.
        
        public DateTime CreatedAt { get; set; }

        
        /// Navigation property for invoice items referencing this product.
        
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        
        /// Constructor initializes default values.
        
        public Product()
        {
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            InvoiceItems = new HashSet<InvoiceItem>();
        }
    }
}