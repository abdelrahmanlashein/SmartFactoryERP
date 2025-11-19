using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
  
    public class PurchaseOrder : BaseAuditableEntity, IAggregateRoot
    {
        public string PONumber { get; private set; }
        public int SupplierID { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime ExpectedDeliveryDate { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }

        // المجموع يحسب بناءً على الأصناف
        public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

        // القائمة الداخلية (قابلة للتعديل)
        private readonly List<PurchaseOrderItem> _items = new();

        // القائمة الخارجية (للقراءة فقط)
        public virtual IReadOnlyCollection<PurchaseOrderItem> Items => _items.AsReadOnly();
        public virtual Supplier Supplier { get; private set; }

        private PurchaseOrder() { }

        public static PurchaseOrder Create(string poNumber, int supplierId, DateTime expectedDate)
        {
            return new PurchaseOrder
            {
                PONumber = poNumber,
                SupplierID = supplierId,
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = expectedDate,
                Status = PurchaseOrderStatus.Draft
            };
        }

        // إضافة صنف للطلب
        public void AddItem(int materialId, int quantity, decimal unitPrice)
        {
            if (Status != PurchaseOrderStatus.Draft)
                throw new Exception("Cannot add items to a confirmed order.");

            var existingItem = _items.FirstOrDefault(i => i.MaterialID == materialId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                _items.Add(PurchaseOrderItem.Create(materialId, quantity, unitPrice));
            }
        }

        public void Confirm()
        {
            if (!_items.Any()) throw new Exception("Cannot confirm empty order.");
            Status = PurchaseOrderStatus.Confirmed;
        }
    }
}
