using SmartFactoryERP.Domain.Entities.HR___Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task AddAttendanceAsync(Attendance attendance, CancellationToken token);
        Task<Attendance> GetActiveAttendanceByEmployeeIdAsync(int employeeId, DateTime date, CancellationToken token);
        Task<List<Attendance>> GetTodayAttendanceAsync(DateTime date, CancellationToken token);

        // عشان الداشبورد (عدد الحاضرين حالياً)
        Task<int> GetCurrentPresentCountAsync(DateTime date, CancellationToken token);
    }
}
