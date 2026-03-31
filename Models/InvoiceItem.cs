using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Genii_Assessment.Models
{
    /// <summary>
    /// Represents a single line item within an invoice.
    /// </summary>
    public class InvoiceItem
    {
        /// <summary>
        /// Primary key of the invoice item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the invoice.
        /// </summary>
        [Required]
        public int InvoiceId { get; set; }

        /// <summary>
        /// Foreign key referencing the product.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of the product in the invoice.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Cost per unit at the time of invoicing.
        /// </summary>
        [Required]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Total cost for this line (Quantity * UnitCost).
        /// </summary>
        [Required]
        public decimal LineTotal { get; set; }

        /// <summary>
        /// Navigation property to the parent invoice.
        /// </summary>
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Navigation property to the product.
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}