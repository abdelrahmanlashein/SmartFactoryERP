using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
    public enum AlertType
    {
        LowStock,
        OutOfStock
    }
    public class StockAlert
    {
        public int AlertID { get; set; } // (PK)
        public int MaterialID { get; set; } // (FK)
        public AlertType AlertType { get; set; }
        public DateTime AlertDate { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedDate { get; set; } // Nullable, as it's only set when resolved

        // Navigation Property
        public virtual Material Material { get; set; }
    }
}
