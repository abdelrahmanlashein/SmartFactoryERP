using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public class SalesOrder : BaseAuditableEntity, IAggregateRoot
    {
        public string OrderNumber { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public SalesOrderStatus Status { get; private set; }

        // Calculated Total
        public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

        private readonly List<SalesOrderItem> _items = new();
        public virtual IReadOnlyCollection<SalesOrderItem> Items => _items.AsReadOnly();

        public virtual Customer Customer { get; private set; }

        private SalesOrder() { }

        public static SalesOrder Create(int customerId, string orderNumber)
        {
            return new SalesOrder
            {
                CustomerId = customerId,
                OrderNumber = orderNumber,
                OrderDate = DateTime.UtcNow,
                Status = SalesOrderStatus.Draft
            };
        }

        public void AddItem(int materialId, int quantity, decimal unitPrice)
        {
            if (Status != SalesOrderStatus.Draft)
                throw new Exception("Cannot add items to a confirmed order.");

            _items.Add(SalesOrderItem.Create(materialId, quantity, unitPrice));
        }
        public void Confirm()
        {
            // Business Rule: Can only confirm orders that are currently in Draft status
            if (Status != SalesOrderStatus.Draft)
            {
                throw new Exception($"Cannot confirm order with status: {Status}");
            }

            // Check if the order has items
            if (!_items.Any())
            {
                throw new Exception("Cannot confirm an empty sales order.");
            }

            Status = SalesOrderStatus.Confirmed;
        }

        public void MarkAsInvoiced()
        {
            if (Status != SalesOrderStatus.Confirmed && Status != SalesOrderStatus.Shipped)
            {
                throw new Exception("Order must be Confirmed or Shipped to generate invoice.");
            }
            Status = SalesOrderStatus.Invoiced;
        }

        // We will add Confirm() logic later to handle Stock Reservation
    }

}
