using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum InvoiceStatus
    {
        Unpaid = 1,
        Paid = 2,
        Overdue = 3,
        Cancelled = 4
    }
}
