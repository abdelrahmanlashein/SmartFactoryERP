using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
    public enum TransactionType
    {
        Purchase,
        Production,
        Sales,
        Adjustment
    }
    public enum ReferenceType
    {
        PurchaseOrder,
        ProductionOrder,
        SalesOrder,
        ManualAdjustment
    }
    public class StockTransaction
    {
        public long TransactionID { get; set; } // (PK) - استخدمت long لأنه قد يتراكم بسرعة
        public int MaterialID { get; set; } // (FK)
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; } // الكمية (قد تكون موجبة أو سالبة حسب نوع الحركة)
        public DateTime TransactionDate { get; set; }
        public long? ReferenceID { get; set; } // (links to PO/Production/Sales Order)
        public ReferenceType? ReferenceType { get; set; }
        public string Notes { get; set; }

        // Navigation Property (لربط الكيانات ببعضها)
        public virtual Material Material { get; set; } 
    }
}
