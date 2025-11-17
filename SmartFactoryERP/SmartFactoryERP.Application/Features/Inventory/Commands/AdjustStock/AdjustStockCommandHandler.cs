using MediatR;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.AdjustStock
{
    public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, Unit>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdjustStockCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الكيان (المادة)
            var material = await _inventoryRepository.GetMaterialByIdAsync(request.MaterialId, cancellationToken);
            if (material == null)
            {
                throw new Exception($"Material with Id {request.MaterialId} was not found.");
            }

            // 2. استدعاء "سلوك" الـ Domain لتعديل الرصيد
            if (request.Quantity > 0)
            {
                material.IncreaseStock(request.Quantity);
            }
            else
            {
                // request.Quantity هنا سالبة، نحولها لموجب للخصم
                material.DecreaseStock(Math.Abs(request.Quantity));
            }

            // 3. إنشاء "حركة مخزون" لتوثيق العملية
            var transaction = StockTransaction.Create(
                request.MaterialId,
                TransactionType.Adjustment, // نوع الحركة: تسوية
                request.Quantity,
                ReferenceType.ManualAdjustment, // المرجع: تسوية يدوية
                null, // لا يوجد ID مرجعي
                request.Notes
            );

            // 4. إضافة الحركة إلى الـ Repository
            await _inventoryRepository.AddStockTransactionAsync(transaction, cancellationToken);

            // 5. حفظ كل التغييرات (تعديل الرصيد + إضافة الحركة) في "ترانزكشن" واحدة
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
