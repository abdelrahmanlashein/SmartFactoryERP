using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public enum SalesOrderStatus
    {
        Draft,
        Confirmed,
        Delivered,
        Cancelled
    }
    public class SalesOrder
    {
        public int SalesOrderID { get; set; } // (PK)
        public string OrderNumber { get; set; }
        public int CustomerID { get; set; } // (FK)
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public SalesOrderStatus Status { get; set; }
        public string CreatedBy { get; set; } // (مؤقتاً string)
        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SalesOrderItem> Items { get; set; }
    }
}
