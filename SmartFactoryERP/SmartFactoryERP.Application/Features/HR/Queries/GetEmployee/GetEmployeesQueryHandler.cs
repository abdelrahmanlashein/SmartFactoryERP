using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetEmployee
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IHRRepository _hrRepository;
        public GetEmployeesQueryHandler(IHRRepository hrRepository) => _hrRepository = hrRepository;

        public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken token)
        {
            var employees = await _hrRepository.GetAllEmployeesAsync(token);
            return employees.Select(e => new EmployeeDto { Id = e.Id, FullName = e.FullName }).ToList();
        }
    }
}
