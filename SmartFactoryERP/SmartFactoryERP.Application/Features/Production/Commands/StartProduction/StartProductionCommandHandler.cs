using MediatR;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.StartProduction
{
    public class StartProductionCommandHandler : IRequestHandler<StartProductionCommand, Unit>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IInventoryRepository _inventoryRepository; // نحتاج المخزون لصرف المواد
        private readonly IUnitOfWork _unitOfWork;

        public StartProductionCommandHandler(
            IProductionRepository productionRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(StartProductionCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب أمر الإنتاج
            var order = await _productionRepository.GetProductionOrderByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new Exception($"Production Order {request.Id} not found.");

            // 2. جلب وصفة التصنيع (BOM) لهذا المنتج
            // (لنعرف ما هي المواد الخام المطلوبة)
            var bomItems = await _productionRepository.GetBOMForProductAsync(order.ProductId, cancellationToken);

            if (bomItems == null || bomItems.Count == 0)
                throw new Exception($"No Bill of Materials (Recipe) found for Product ID {order.ProductId}. Cannot produce.");

            // 3. حجز وصرف المواد الخام من المخزون
            foreach (var bomItem in bomItems)
            {
                // حساب الكمية المطلوبة = (كمية الوصفة للقطعة الواحدة) * (عدد القطع المطلوب إنتاجها)
                var requiredQuantity = bomItem.Quantity * order.Quantity;

                // جلب المادة الخام من المخزون
                var material = await _inventoryRepository.GetMaterialByIdAsync(bomItem.ComponentId, cancellationToken);
                if (material == null)
                    throw new Exception($"Component Material {bomItem.ComponentId} not found in inventory.");

                // محاولة خصم الكمية (الكيان الذكي سيرمي خطأ لو الرصيد غير كافٍ)
                try
                {
                    // ✅ الآن نمرر decimal مباشرة بدون تحويل
                    material.DecreaseStock(requiredQuantity);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Insufficient stock for material '{material.MaterialName}'. Required: {requiredQuantity:F2}, Available: {material.CurrentStockLevel:F2}");
                }
            }

            // 4. تغيير حالة الأمر إلى "Started"
            order.StartProduction();

            // 5. حفظ كل التغييرات (تحديث حالة الأمر + خصم أرصدة المواد)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}