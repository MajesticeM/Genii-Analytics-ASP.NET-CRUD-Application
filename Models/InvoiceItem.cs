using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Genii_Assessment.Models
{
     
    /// Represents a single line item within an invoice.
     
    public class InvoiceItem
    {
         
        /// Primary key of the invoice item.
         
        public int Id { get; set; }

         
        /// Foreign key referencing the invoice.
         
        [Required]
        public int InvoiceId { get; set; }

         
        /// Foreign key referencing the product.
         
        [Required]
        public int ProductId { get; set; }

         
        /// Quantity of the product in the invoice.
         
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

         
        /// Cost per unit at the time of invoicing.
         
        [Required]
        public decimal UnitCost { get; set; }

         
        /// Total cost for this line (Quantity * UnitCost).
         
        [Required]
        public decimal LineTotal { get; set; }

         
        /// Navigation property to the parent invoice.
         
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

         
        /// Navigation property to the product.
         
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}