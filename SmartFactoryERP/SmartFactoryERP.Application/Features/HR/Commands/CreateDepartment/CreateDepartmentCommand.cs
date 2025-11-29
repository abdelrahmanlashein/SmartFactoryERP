using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Code { get; set; } // e.g., "IT", "HR", "SALES"
        public string Description { get; set; }
    }
}
