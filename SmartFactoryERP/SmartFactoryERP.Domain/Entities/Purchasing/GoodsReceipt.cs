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

    public class GoodsReceipt
    {
        public int ReceiptID { get; set; } // (PK)
        public int PurchaseOrderID { get; set; } // (FK)
        public DateTime ReceiptDate { get; set; }
        public string ReceivedBy { get; set; } // (مؤقتاً string، قد نربطه لاحقاً بـ Employee)
        public string Notes { get; set; }
        public GoodsReceiptStatus Status { get; set; }

        // Navigation Properties
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual ICollection<GoodsReceiptItem> Items { get; set; }
    }
}
