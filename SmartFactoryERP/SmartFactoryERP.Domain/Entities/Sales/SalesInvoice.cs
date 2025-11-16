using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public enum InvoiceStatus
    {
        Pending,
        Paid
    }
    public class SalesInvoice
    {
        public int InvoiceID { get; set; } // (PK)
        public int SalesOrderID { get; set; } // (FK)
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }

        // Navigation Property
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
