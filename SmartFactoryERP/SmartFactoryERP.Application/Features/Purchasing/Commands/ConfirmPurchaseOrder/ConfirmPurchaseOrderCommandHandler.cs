using MediatR;
using SmartFactoryERP.Domain.Enums; // للتأكد من الحالة
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.ConfirmPurchaseOrder
{
    public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, Unit>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPurchaseOrderCommandHandler(IPurchasingRepository purchasingRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الطلب مع الأصناف (ضروري لأن الـ Domain بيتشيك على الأصناف)
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new Exception($"Purchase Order with Id {request.Id} was not found.");
            }

            // (Check) لو الطلب أصلاً مؤكد، مفيش داعي نعمل حاجة (Idempotency)
            if (order.Status == PurchaseOrderStatus.Confirmed)
            {
                return Unit.Value;
            }

            // 2. تنفيذ منطق البيزنس (تغيير الحالة + التحقق من الأصناف)
            // هذه الدالة سترمي Exception لو الأصناف فارغة
            order.Confirm();

            // 3. ⚠ أهم سطر: حفظ التغييرات في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}