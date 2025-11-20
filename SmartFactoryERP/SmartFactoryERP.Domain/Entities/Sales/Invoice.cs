using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public class Invoice : BaseAuditableEntity, IAggregateRoot
    {
        public string InvoiceNumber { get; private set; }
        public int SalesOrderId { get; private set; }
        public DateTime InvoiceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public InvoiceStatus Status { get; private set; }

        // Navigation Property
        public virtual SalesOrder SalesOrder { get; private set; }

        private Invoice() { }

        public static Invoice Create(int salesOrderId, decimal totalAmount, DateTime dueDate)
        {
            return new Invoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                SalesOrderId = salesOrderId,
                TotalAmount = totalAmount,
                InvoiceDate = DateTime.UtcNow,
                DueDate = dueDate,
                Status = InvoiceStatus.Unpaid
            };
        }

        public void MarkAsPaid()
        {
            Status = InvoiceStatus.Paid;
        }
    }
}
