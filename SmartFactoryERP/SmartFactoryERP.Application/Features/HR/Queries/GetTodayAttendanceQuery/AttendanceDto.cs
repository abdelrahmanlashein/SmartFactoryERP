using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetTodayAttendanceQuery
{
    public class AttendanceDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Status { get; set; } // Present / Left
    }
}
