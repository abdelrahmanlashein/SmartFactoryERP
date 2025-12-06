using MediatR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.ManageAttendance
{
    public class ToggleAttendanceCommandHandler : IRequestHandler<ToggleAttendanceCommand, string>
    {
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IUnitOfWork _uow;

        public ToggleAttendanceCommandHandler(IAttendanceRepository attendanceRepo, IUnitOfWork uow)
        {
            _attendanceRepo = attendanceRepo;
            _uow = uow;
        }

        public async Task<string> Handle(ToggleAttendanceCommand request, CancellationToken token)
        {
            var today = DateTime.UtcNow.Date;

            // 1. هل الموظف موجود حالياً؟ (عمل دخول ولم يخرج)
            var activeRecord = await _attendanceRepo.GetActiveAttendanceByEmployeeIdAsync(request.EmployeeId, today, token);

            if (activeRecord != null)
            {
                // -- سيناريو الخروج --
                activeRecord.CheckOut();
                await _uow.SaveChangesAsync(token);
                return "Checked Out 🚪";
            }
            else
            {
                // -- سيناريو الدخول --
                var newRecord = Attendance.CreateCheckIn(request.EmployeeId);
                await _attendanceRepo.AddAttendanceAsync(newRecord, token);
                await _uow.SaveChangesAsync(token);
                return "Checked In ✅";
            }
        }
    }
}
