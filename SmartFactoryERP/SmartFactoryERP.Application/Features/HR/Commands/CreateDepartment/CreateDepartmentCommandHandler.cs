using MediatR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IHRRepository _hrRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepartmentCommandHandler(IHRRepository hrRepository, IUnitOfWork unitOfWork)
        {
            _hrRepository = hrRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء الكيان (Entity)
            var department = Department.Create(request.Name, request.Code, request.Description);

            // 2. الإضافة للـ Repository
            await _hrRepository.AddDepartmentAsync(department, cancellationToken);

            // 3. الحفظ في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return department.Id;
        }
    }
}
