using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class ChangeTaskStatusCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string NewStatus { get; set; } // "Start" or "Complete"
    }
}
