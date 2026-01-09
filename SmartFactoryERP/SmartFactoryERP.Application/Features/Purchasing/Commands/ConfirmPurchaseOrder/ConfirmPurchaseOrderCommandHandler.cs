using MediatR;
using SmartFactoryERP.Domain.Enums; // for checking status
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
            // 1. Fetch the purchase order with its items (necessary because Domain checks items)
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new Exception($"Purchase Order with Id {request.Id} was not found.");
            }

            // (Check) If the order is already confirmed, do nothing (idempotency)
            if (order.Status == PurchaseOrderStatus.Confirmed)
            {
                return Unit.Value;
            }

            // 2. Execute business logic (change status + validate items)
            // This method will throw Exception if items are empty
            order.Confirm();

            // 3. ⚠ Important: persist changes to the database
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}