using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Genii_Assessment.ViewModels
{
   
    /// Represents a single invoice item row.
    public class InvoiceItemInputViewModel
    {
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        
        [Display(Name = "Line Total")]
        public decimal LineTotal { get; set; }

        public IEnumerable<SelectListItem> ProductOptions { get; set; }
    }
}