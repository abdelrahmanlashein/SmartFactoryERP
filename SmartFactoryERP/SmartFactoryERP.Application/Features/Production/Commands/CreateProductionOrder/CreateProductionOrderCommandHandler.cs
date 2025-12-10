using MediatR;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateProductionOrder
{
    public class CreateProductionOrderCommandHandler : IRequestHandler<CreateProductionOrderCommand, int>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductionOrderCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateProductionOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء الأوردر الأساسي
            var order = ProductionOrder.Create(
                request.ProductId,
                request.Quantity,
                request.StartDate,
                request.Notes,
                request.Priority ?? "Medium"
            );

            // ✅✅ 2. إلغاء البحث عن BOM قديمة واستخدام الخامات المرسلة (request.Items) ✅✅

            if (request.Items == null || !request.Items.Any())
            {
                // لو الفرونت إند مبعتش أي خامات، يبقى فيه مشكلة
                throw new Exception("Cannot create order. Please specify required raw materials (Items list is empty).");
            }

            // 3. إضافة الخامات المرسلة مباشرة للأوردر
            foreach (var item in request.Items)
            {
                // ✅ ملاحظة: هنا نفترض أن Quantity المرسلة هي الكمية الإجمالية النهائية
                //             ولا نحتاج لضربها في كمية الأوردر request.Quantity
                order.AddOrUpdateItem(item.MaterialId, item.Quantity);
            }

            // 4. الحفظ
            await _productionRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}