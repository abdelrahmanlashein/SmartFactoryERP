using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Performance___Task_Management
{
    public class EmployeePerformance
    {
        public int PerformanceID { get; set; } // (PK)
        public int EmployeeID { get; set; } // (FK)

        // (Month/Year) - سأستخدم string لتخزينها
        public string EvaluationPeriod { get; set; }

        public double TaskCompletionRate { get; set; }
        public double AttendanceScore { get; set; }
        public double OverallScore { get; set; }
        public DateTime CalculatedDate { get; set; }
        public string Notes { get; set; }

        // Navigation Property
        public virtual Employee Employee { get; set; }
    }
}
