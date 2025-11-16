using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Inventory
{
   
    public class StockAlert:BaseEntity
    {
        public int MaterialID { get; private set; } // (FK)
        public Material Material { get; private set; }
        public AlertType AlertType { get; private set; }
        public DateTime AlertDate { get; private set; }
        public bool IsResolved { get; private set; }
        public DateTime? ResolvedDate { get; private set; }

        // ... (يمكن إضافة Methods مثل ResolveAlert()) ...   
    }
}
