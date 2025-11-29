using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetEmployee
{
    public class GetEmployeesQuery : IRequest<List<EmployeeDto>> { }
}
