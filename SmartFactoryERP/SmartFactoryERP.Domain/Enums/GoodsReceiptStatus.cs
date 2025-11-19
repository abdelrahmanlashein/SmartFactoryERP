using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Enums
{
    public enum GoodsReceiptStatus
    {
        Partial = 1,  // استلام جزئي (لسه باقي كميات في أمر الشراء)
        Complete = 2  // استلام كلي (كل الكميات المطلوبة وصلت)
    }
}
