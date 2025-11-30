using MediatR;
using SmartFactoryERP.Domain.Entities.HR;
using SmartFactoryERP.Domain.Entities.HR___Departments;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.HR.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IHRRepository _hrRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeCommandHandler(IHRRepository hrRepository, IUnitOfWork unitOfWork)
        {
            _hrRepository = hrRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء الكيان وربطه بالقسم (DepartmentId)
            var employee = Employee.Create(
                request.FullName,
                request.JobTitle,
                request.Email,
                request.PhoneNumber,
                request.DepartmentId
            );

            // 2. الإضافة للـ Repository
            await _hrRepository.AddEmployeeAsync(employee, cancellationToken);

            // 3. الحفظ (لو القسم غير موجود، قاعدة البيانات هترجع Error بسبب الـ Foreign Key)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return employee.Id;
        }
    }
}
