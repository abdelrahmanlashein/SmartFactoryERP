using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetPerformance
{
    public class EmployeePerformanceDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }

        // مؤشرات الأداء (KPIs)
        public double CompletionRate { get; set; } // نسبة الإنجاز %
        public string PerformanceLabel { get; set; } // "Excellent", "Good", "Needs Improvement"
    }
}
