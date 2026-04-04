using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Genii_Assessment.Models
{
     
    /// Represents an invoice created by a user.
     
    public class Invoice
    {
         
        /// Primary key of the invoice.
         
        public int Id { get; set; }

         
        /// Date the invoice was created.
         
        [Required]
        public DateTime InvoiceDate { get; set; }

         
        /// Total amount of the invoice.
         
        [Required]
        public decimal TotalAmount { get; set; }

         
        /// Foreign key referencing the user who created the invoice.
         
        [Required]
        [StringLength(128)]
        public string CreatedByUserId { get; set; }

         
        /// Date the invoice was created.
         
        public DateTime CreatedAt { get; set; }

         
        /// Date the invoice was last updated.
         
        public DateTime? UpdatedAt { get; set; }

         
        /// Navigation property to the user who created the invoice.
         
        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser CreatedByUser { get; set; }

         
        /// Collection of invoice items belonging to this invoice.
         
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

         
        /// Constructor initializes default values.
         
        public Invoice()
        {
            InvoiceDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            InvoiceItems = new HashSet<InvoiceItem>();
        }
    }
}