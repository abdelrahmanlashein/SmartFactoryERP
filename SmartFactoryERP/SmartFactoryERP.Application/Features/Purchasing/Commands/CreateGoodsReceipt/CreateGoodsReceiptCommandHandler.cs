using MediatR;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Application.Interfaces.Identity; 
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateGoodsReceipt
{
    public class CreateGoodsReceiptCommandHandler : IRequestHandler<CreateGoodsReceiptCommand, int>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        // 👇 1. إضافة خدمة المستخدم الحالي
        private readonly ICurrentUserService _currentUserService;

        public CreateGoodsReceiptCommandHandler(
            IPurchasingRepository purchasingRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService) // 👈 الحقن هنا
        {
            _purchasingRepository = purchasingRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من أمر الشراء
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.PurchaseOrderId, cancellationToken);
            if (order == null)
            {
                throw new Exception($"Purchase Order {request.PurchaseOrderId} not found.");
            }
            if (order.Status != Domain.Enums.PurchaseOrderStatus.Confirmed &&
                order.Status != Domain.Enums.PurchaseOrderStatus.PartiallyReceived)
            {
                throw new Exception($"Cannot receive goods for order status: {order.Status}.");
            }

            // 👇 2. الحصول على رقم الموظف من التوكن (الذكاء هنا)
            // 👇 2. استخدام الموظف المحدد من الـ request
            var employeeId = request.ReceivedById;

            if (employeeId <= 0)
            {
                throw new Exception("Invalid Employee ID. Please select who received the goods.");
            }

            // 3. إنشاء إذن الاستلام باستخدام ID الموظف الأوتوماتيكي
            var receipt = GoodsReceipt.Create(
                request.PurchaseOrderId,
                employeeId, // ✅ نستخدم القيمة من التوكن
                request.Notes
            );

            // 4. معالجة الأصناف وتحديث المخزون
            foreach (var itemDto in request.Items.Where(i => i.ReceivedQuantity > 0))
            {
                var poItem = order.Items.FirstOrDefault(i => i.Id == itemDto.POItemId);
                if (poItem == null) continue;

                var material = await _inventoryRepository.GetMaterialByIdAsync(poItem.MaterialID, cancellationToken);

                // زيادة المخزون
                material.IncreaseStock(itemDto.ReceivedQuantity);

                // إضافة الصنف للاستلام
                receipt.AddReceivedItem(itemDto.POItemId, itemDto.ReceivedQuantity, itemDto.RejectedQuantity);
            }

            // 5. الحفظ
            await _purchasingRepository.AddGoodsReceiptAsync(receipt, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return receipt.Id;
        }
    }
}