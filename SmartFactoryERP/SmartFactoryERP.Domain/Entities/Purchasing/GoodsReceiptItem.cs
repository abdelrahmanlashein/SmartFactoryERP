using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public class GoodsReceiptItem : BaseEntity
    {
        public int ReceiptID { get; private set; }
        public int POItemID { get; private set; } // رابط مع سطر أمر الشراء
        public int ReceivedQuantity { get; private set; }
        public int RejectedQuantity { get; private set; }

        // Navigation Properties
        public virtual GoodsReceipt GoodsReceipt { get; private set; }
        public virtual PurchaseOrderItem PurchaseOrderItem { get; private set; }

        private GoodsReceiptItem() { }

        internal static GoodsReceiptItem Create(int poItemId, int receivedQty, int rejectedQty)
        {
            return new GoodsReceiptItem
            {
                POItemID = poItemId,
                ReceivedQuantity = receivedQty,
                RejectedQuantity = rejectedQty
            };
        }
    }
}
