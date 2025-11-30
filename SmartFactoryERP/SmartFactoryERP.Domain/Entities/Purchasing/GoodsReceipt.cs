using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public enum GoodsReceiptStatus
    {
        Partial,
        Complete
    }

    public class GoodsReceipt : BaseAuditableEntity, IAggregateRoot
    {
        public int PurchaseOrderID { get; private set; }
        public DateTime ReceiptDate { get; private set; }
        //public String ReceivedBy { get; private set; }
        public int ReceivedById { get; private set; }
        public virtual Employee ReceivedBy { get; private set; } // Navigation Property
        public string Notes { get; private set; }
        public GoodsReceiptStatus Status { get; private set; }

        // إدارة العناصر داخلياً
        private readonly List<GoodsReceiptItem> _items = new();
        public virtual IReadOnlyCollection<GoodsReceiptItem> Items => _items.AsReadOnly();

        // Navigation Property
        public virtual PurchaseOrder PurchaseOrder { get; private set; }

        private GoodsReceipt() { }

        public static GoodsReceipt Create(int purchaseOrderId, int employeeId, string notes)
        {
            return new GoodsReceipt
            {
                PurchaseOrderID = purchaseOrderId,
                ReceiptDate = DateTime.UtcNow,
                ReceivedById = employeeId,
                Notes = notes,
                Status = GoodsReceiptStatus.Partial // نبدأ بجزئي حتى يكتمل
            };
        }

        public void AddReceivedItem(int poItemId, int receivedQty, int rejectedQty)
        {
            if (receivedQty < 0 || rejectedQty < 0)
                throw new Exception("Quantities cannot be negative.");

            if (receivedQty == 0 && rejectedQty == 0)
                throw new Exception("Must receive or reject at least one item.");

            var item = GoodsReceiptItem.Create(poItemId, receivedQty, rejectedQty);
            _items.Add(item);
        }

        // يمكن إضافة ميثود هنا لاحقاً لتحديث الـ Status
        // بناءً على مقارنة الكميات المستلمة بالكميات المطلوبة في الـ PO
        public void FinalizeReceipt(bool isComplete)
        {
            Status = isComplete ? GoodsReceiptStatus.Complete : GoodsReceiptStatus.Partial;
        }
    }
}
