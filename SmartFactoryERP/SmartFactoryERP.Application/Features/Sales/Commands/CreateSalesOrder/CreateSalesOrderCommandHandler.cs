using MediatR;
using SmartFactoryERP.Domain.Entities.Sales;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.CreateSalesOrder
{
    public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, int>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSalesOrderCommandHandler(ISalesRepository salesRepository, IUnitOfWork unitOfWork)
        {
            _salesRepository = salesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من وجود العميل
            var customer = await _salesRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null || !customer.IsActive)
            {
                throw new Exception("Invalid or inactive customer.");
            }

            // 2. إنشاء رقم طلب مميز (SO-yyyyMMdd-GUID)
            var orderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            // 3. إنشاء الـ Header
            var order = SalesOrder.Create(request.CustomerId, orderNumber);

            // 4. إضافة الأصناف
            foreach (var item in request.Items)
            {
                order.AddItem(item.MaterialId, item.Quantity, item.UnitPrice);
            }

            // 5. الحفظ
            await _salesRepository.AddSalesOrderAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
