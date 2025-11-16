using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late,
        Leave
    }
    public class Attendance
    {
        public int AttendanceID { get; set; } // (PK)
        public int EmployeeID { get; set; } // (FK)
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; } // (Nullable في حالة الغياب/الإجازة)
        public DateTime? CheckOutTime { get; set; } // (Nullable)
        public double WorkingHours { get; set; } // (يمكن حسابه)
        public AttendanceStatus Status { get; set; }

        // Navigation Property
        public virtual Employee Employee { get; set; }
    }
}
