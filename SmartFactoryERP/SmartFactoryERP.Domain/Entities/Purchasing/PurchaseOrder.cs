using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public enum PurchaseOrderStatus
    {
        Draft,
        Confirmed,
        Received,
        Cancelled
    }
    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; } // (PK)
        public string PONumber { get; set; }
        public int SupplierID { get; set; } // (FK)
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseOrderStatus Status { get; set; }

        // (CreatedBy - سأفترض أنه اسم المستخدم أو ID المستخدم، سأستخدم string مؤقتاً)
        // (يمكن تغييره لاحقاً إلى FK لجدول المستخدمين)
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseOrderItem> Items { get; set; }
    }
}
