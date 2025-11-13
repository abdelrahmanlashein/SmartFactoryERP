using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public class SalesOrderItem
    {
        public int SOItemID { get; set; } // (PK)
        public int SalesOrderID { get; set; } // (FK)
        public int MaterialID { get; set; } // (FK)  [Finished Goods only]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Navigation Properties
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Material Material { get; set; }
    }
}
