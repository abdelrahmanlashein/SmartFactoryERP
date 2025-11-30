using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum WorkTaskStatus
    {
        Pending = 1,      // معلقة
        InProgress = 2,   // جاري العمل
        Completed = 3,    // مكتملة
        Cancelled = 4     // ملغاة
    }
}
