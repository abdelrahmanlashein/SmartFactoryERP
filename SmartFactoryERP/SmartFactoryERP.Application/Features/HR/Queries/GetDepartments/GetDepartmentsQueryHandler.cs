using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Queries.GetDepartments
{
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentDto>>
    {
        private readonly IHRRepository _hrRepo;
        
        public GetDepartmentsQueryHandler(IHRRepository hrRepo) => _hrRepo = hrRepo;

        public async Task<List<DepartmentDto>> Handle(GetDepartmentsQuery req, CancellationToken token)
        {
            var departments = await _hrRepo.GetAllDepartmentsAsync(token);
            
            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description,
                EmployeeCount = d.Employees?.Count ?? 0
            }).ToList();
        }
    }
}
