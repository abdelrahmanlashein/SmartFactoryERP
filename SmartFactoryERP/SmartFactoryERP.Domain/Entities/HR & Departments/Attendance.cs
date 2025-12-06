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

    public class Attendance : BaseEntity
    {
        public int EmployeeId { get; private set; }
        public DateTime Date { get; private set; } // تاريخ اليوم (بدون وقت)
        public DateTime? CheckInTime { get; private set; }
        public DateTime? CheckOutTime { get; private set; }

        // خاصية محسوبة: هل هو متواجد الآن؟
        public bool IsPresent => CheckInTime.HasValue && !CheckOutTime.HasValue;

        // Navigation
        public virtual Employee Employee { get; private set; }

        private Attendance() { }

        // تسجيل دخول
        public static Attendance CreateCheckIn(int employeeId)
        {
            return new Attendance
            {
                EmployeeId = employeeId,
                Date = DateTime.UtcNow.Date,
                CheckInTime = DateTime.UtcNow
            };
        }

        // تسجيل خروج
        public void CheckOut()
        {
            if (CheckOutTime.HasValue)
                throw new Exception("Employee already checked out.");

            CheckOutTime = DateTime.UtcNow;
        }
    }
}
