using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetTodayAttendanceQuery
{
    public class GetTodayAttendanceQueryHandler : IRequestHandler<GetTodayAttendanceQuery, List<AttendanceDto>>
    {
        private readonly IAttendanceRepository _attendanceRepo;
        public GetTodayAttendanceQueryHandler(IAttendanceRepository repo) => _attendanceRepo = repo;

        public async Task<List<AttendanceDto>> Handle(GetTodayAttendanceQuery req, CancellationToken token)
        {
            var logs = await _attendanceRepo.GetTodayAttendanceAsync(DateTime.UtcNow.Date, token);

            return logs.Select(a => new AttendanceDto
            {
                EmployeeId = a.EmployeeId,
                EmployeeName = a.Employee.FullName,
                CheckIn = a.CheckInTime,
                CheckOut = a.CheckOutTime,
                Status = a.CheckOutTime == null ? "🟢 Present" : "🔴 Left"
            }).ToList();
        }
    }
}
