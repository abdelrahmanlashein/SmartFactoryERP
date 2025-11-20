using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum SalesOrderStatus
    {
        Draft = 1,          // Quote/Initial Order
        Confirmed = 2,      // Order accepted, stock reserved
        Shipped = 3,        // Goods left the warehouse
        Invoiced = 4,       // Bill sent to customer
        Cancelled = 5
    }
}
