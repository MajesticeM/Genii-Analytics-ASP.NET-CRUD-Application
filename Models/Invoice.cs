using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Genii_Assessment.Models
{
    /// <summary>
    /// Represents an invoice created by a user.
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Primary key of the invoice.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date the invoice was created.
        /// </summary>
        [Required]
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Total amount of the invoice.
        /// </summary>
        [Required]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Foreign key referencing the user who created the invoice.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string CreatedByUserId { get; set; }

        /// <summary>
        /// Date the invoice was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date the invoice was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property to the user who created the invoice.
        /// </summary>
        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        /// <summary>
        /// Collection of invoice items belonging to this invoice.
        /// </summary>
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        /// <summary>
        /// Constructor initializes default values.
        /// </summary>
        public Invoice()
        {
            InvoiceDate = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            InvoiceItems = new HashSet<InvoiceItem>();
        }
    }
}