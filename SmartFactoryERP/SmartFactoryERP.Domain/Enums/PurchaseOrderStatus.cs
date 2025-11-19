using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum PurchaseOrderStatus
    {
        Draft = 1,          // مسودة (تحت الإنشاء)
        PendingApproval = 2,// في انتظار الموافقة (لو فيه دورة موافقات)
        Confirmed = 3,      // تم التأكيد وإرساله للمورد
        PartiallyReceived = 4, // تم استلام جزء من البضاعة
        Completed = 5,      // تم استلام كل البضاعة وإغلاق الطلب
        Cancelled = 6       // تم إلغاء الطلب
    }
}
