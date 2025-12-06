using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        public AttendanceRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAttendanceAsync(Attendance attendance, CancellationToken token)
        {
            await _context.Attendances.AddAsync(attendance, token);
        }

        public async Task<Attendance> GetActiveAttendanceByEmployeeIdAsync(int employeeId, DateTime date, CancellationToken token)
        {
            // يبحث عن سجل دخول اليوم ولم يتم تسجيل الخروج له بعد
            return await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == date && a.CheckOutTime == null, token);
        }

        public async Task<List<Attendance>> GetTodayAttendanceAsync(DateTime date, CancellationToken token)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Date == date)
                .OrderByDescending(a => a.CheckInTime)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public async Task<int> GetCurrentPresentCountAsync(DateTime date, CancellationToken token)
        {
            return await _context.Attendances
                .CountAsync(a => a.Date == date && a.CheckOutTime == null, token);
        }
    }
}
