using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.ManageAttendance
{
    public class ToggleAttendanceCommand : IRequest<string> // يرجع رسالة (Checked In / Checked Out)
    {
        public int EmployeeId { get; set; }
    }
}
