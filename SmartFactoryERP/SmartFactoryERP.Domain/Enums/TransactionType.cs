using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum TransactionType
    {
        Purchase = 1,      // استلام مشتريات
        Production = 2,    // استلام منتج تام
        Sales = 3,         // صرف مبيعات
        Consumption = 4,   // صرف مواد خام للإنتاج
        Adjustment = 5     // تسوية جرد
    }
}
