using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum ProductionStatus
    {
        Planned = 1,    // مخطط له (لم يبدأ بعد)
        Started = 2,    // قيد التصنيع (المواد محجوزة)
        Completed = 3,  // تم الانتهاء (المنتج دخل المخزون)
        Cancelled = 4
    }
}
