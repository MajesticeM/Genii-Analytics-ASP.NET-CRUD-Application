using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Genii_Assessment.ViewModels
{
    /// <summary>
    /// View model used to create and edit invoices.
    /// </summary>
    public class InvoiceCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        // Display-only. Do not rely on posted value.
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        public List<InvoiceItemInputViewModel> Items { get; set; }

        public InvoiceCreateEditViewModel()
        {
            InvoiceDate = DateTime.UtcNow;
            Items = new List<InvoiceItemInputViewModel>();
        }
    }
}