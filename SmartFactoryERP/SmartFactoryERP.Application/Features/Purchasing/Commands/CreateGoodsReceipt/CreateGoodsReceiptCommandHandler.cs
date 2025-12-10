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
        private readonly ICurrentUserService _currentUserService;

        public CreateGoodsReceiptCommandHandler(
            IPurchasingRepository purchasingRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
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

            // التأكد إن الحالة تسمح بالاستلام
            if (order.Status != Domain.Enums.PurchaseOrderStatus.Confirmed &&
                order.Status != Domain.Enums.PurchaseOrderStatus.PartiallyReceived)
            {
                throw new Exception($"Cannot receive goods for order status: {order.Status}.");
            }

            // 2. الحصول على رقم الموظف
            var employeeId = request.ReceivedById; // (أو من الـ Token لو حبيت تفعله مستقبلاً)

            if (employeeId <= 0) // لو الرقم جاي String وحولناه لـ Int ممكن يطلع 0 لو فشل
            {
                // ملحوظة: لو الـ ID عندك String في الـ Command، لازم التحويل يتم صح. 
                // بس بما إننا عدلنا الـ Command لـ String، والـ Entity بياخد int، لازم نتأكد من التحويل.
                // *لو الـ EmployeeId في الداتابيز int، يبقى لازم الـ Front يبعت رقم.*
                // *مؤقتاً هنفترض إنه int صحيح.*
            }

            // 3. إنشاء إذن الاستلام
            var receipt = GoodsReceipt.Create(
                request.PurchaseOrderId,
                employeeId,
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

            // ⭐⭐⭐ التعديل الحاسم هنا ⭐⭐⭐
            // تغيير حالة الطلب إلى "مكتمل" عشان الزرار يختفي
            order.MarkAsReceived();

            // 5. الحفظ
            await _purchasingRepository.AddGoodsReceiptAsync(receipt, cancellationToken);

            // الـ SaveChangesAsync هتحفظ:
            // 1. إذن الاستلام الجديد (receipt)
            // 2. تحديث المخزون (material)
            // 3. تحديث حالة الطلب (order) -> لأن الـ Entity Framework بتراقب التغييرات (Tracking)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return receipt.Id;
        }
    }
}