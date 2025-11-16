using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Production
{
    public enum ProductionOrderStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    public class ProductionOrder
    {
        public int ProductionOrderID { get; set; } // (PK)
        public string OrderNumber { get; set; }
        public int MaterialID { get; set; } // (FK) [Finished Good to produce]
        public int PlannedQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; } // Nullable
        public ProductionOrderStatus Status { get; set; }
        public string CreatedBy { get; set; } // (مؤقتاً string)
        public DateTime CreatedDate { get; set; }

        // Navigation Property
        public virtual Material Material { get; set; }
    }
}
