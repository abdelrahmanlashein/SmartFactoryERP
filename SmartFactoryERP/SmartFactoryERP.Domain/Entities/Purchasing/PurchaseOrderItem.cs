using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public class PurchaseOrderItem
    {
        public int POItemID { get; set; } // (PK)
        public int PurchaseOrderID { get; set; } // (FK)
        public int MaterialID { get; set; } // (FK)
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // (Quantity * UnitPrice)

        // Navigation Properties
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Material Material { get; set; }
    }
}
