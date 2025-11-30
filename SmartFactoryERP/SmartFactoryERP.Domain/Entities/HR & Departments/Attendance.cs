using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.Shared;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.HR___Departments
{
   
    public class Attendance :BaseEntity
    {
        // ID (PK) is from BaseEntity
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
