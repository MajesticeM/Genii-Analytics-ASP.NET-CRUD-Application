using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Genii_Assessment.ViewModels
{
    /// View model used to create and edit invoices.
    public class InvoiceCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
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