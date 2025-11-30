using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetPerformance
{
    public class GetPerformanceQuery : IRequest<List<EmployeePerformanceDto>>
    {
        public int? EmployeeId { get; set; } // اختياري
    }
}
