using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Models.AI
{
    // هذا الكلاس يمثل "نقطة بيانات" واحدة في التاريخ
    public class SalesHistoryRecord
    {
        public DateTime Date { get; set; }
        public float TotalQuantity { get; set; }
    }
}
