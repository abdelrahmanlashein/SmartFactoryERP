using MediatR;
using SmartFactoryERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; } // Low, Medium, High...
        public int? AssignedEmployeeId { get; set; } // الموظف المسؤول
    }
}
