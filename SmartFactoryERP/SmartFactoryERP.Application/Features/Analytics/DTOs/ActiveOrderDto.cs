using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.DTOs
{
    public class ActiveOrderDto
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; } // عشان نحسب الوقت المتبقي
        public string Status { get; set; }

        // خاصية محسوبة لنسبة الإنجاز الزمنية (بديلاً عن الكمية حالياً)
        public int TimeProgressPercentage
        {
            get
            {
                if (EstimatedEndDate == null) return 0;
                var totalTime = (EstimatedEndDate.Value - StartDate).TotalMinutes;
                var elapsedTime = (DateTime.UtcNow - StartDate).TotalMinutes;
                if (totalTime <= 0) return 0;
                var percent = (int)((elapsedTime / totalTime) * 100);
                return Math.Min(percent, 99); // لا تتجاوز 99% حتى تكتمل
            }
        }
    }
}
