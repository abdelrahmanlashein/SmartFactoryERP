using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Models.AI
{
    // هذا الكلاس يمثل نتيجة التوقع
    public class ForecastResult
    {
        public float PredictedSales { get; set; } // الرقم المتوقع
        public float LowerBound { get; set; }     // الحد الأدنى (أسوأ الظروف)
        public float UpperBound { get; set; }     // الحد الأقصى (أحسن الظروف)
    }
}
