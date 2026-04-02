using System;

namespace Genii_Assessment.ViewModels
{
   
    /// Represents an invoice in the list view.
    public class InvoiceListItemViewModel
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
}