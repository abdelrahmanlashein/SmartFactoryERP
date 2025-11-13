using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public class GoodsReceiptItem
    {
        public int ReceiptItemID { get; set; } // (PK)
        public int ReceiptID { get; set; } // (FK)
        public int POItemID { get; set; } // (FK)
        public int ReceivedQuantity { get; set; }
        public int RejectedQuantity { get; set; }

        // Navigation Properties
        public virtual GoodsReceipt GoodsReceipt { get; set; }
        public virtual PurchaseOrderItem PurchaseOrderItem { get; set; }
    }
}
