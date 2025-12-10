using MediatR;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq; // ضروري لـ Any()

namespace SmartFactoryERP.Application.Features.Production.Commands.StartProduction
{
    public class StartProductionCommandHandler : IRequestHandler<StartProductionCommand, Unit>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IInventoryRepository _inventoryRepository;
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
            // 1. جلب أمر الإنتاج مع الخامات (Items)
            // ✅✅ تم استخدام request.Id ليتطابق مع الـ Command ✅✅
            var order = await _productionRepository.GetOrderWithItemsAsync(request.Id);

            if (order == null)
                throw new Exception($"Production Order {request.Id} not found.");

            // التأكد من أن الأوردر في حالة تسمح بالبدء
            if (order.Status != Domain.Enums.ProductionStatus.Planned)
                throw new Exception($"Cannot start order. Current status is {order.Status}.");


            // 2. خصم المواد الخام من المخزون بناءً على الخامات المنسوخة داخل الأوردر (order.Items)
            if (order.Items == null || !order.Items.Any())
            {
                throw new Exception($"Order {order.OrderNumber} has no materials defined (BOM is empty). Cannot start production.");
            }

            foreach (var item in order.Items)
            {
                // الخصم يتم هنا (لو الرصيد مش كافي، الدالة دي هترمي Exception من الريبوزيتوري)
                await _inventoryRepository.DeductStockAsync(item.MaterialId, item.Quantity, cancellationToken);
            }

            // 3. تغيير حالة الأمر إلى "Started"
            order.StartProduction();

            // 4. حفظ كل التغييرات (يضمن أن الخصم وحالة الأوردر تحدث في نفس الـ Transaction)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}