using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // 1. Fetch the Purchase Order Aggregate Root (Needs items loaded for validation!)
            // We reuse GetPurchaseOrderWithItemsAsync
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new Exception($"Purchase Order with Id {request.Id} was not found.");
            }

            // 2. Execute the Domain method (Confirms the order and enforces business rules like "Cannot confirm empty order")
            order.Confirm();

            // 3. Save changes (EF Core tracks the status change)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Note: In a real system, a Domain Event (e.g., PurchaseOrderConfirmedEvent) would be raised here 
            // to trigger email notifications, budget allocation, etc.

            return Unit.Value;
        }
        
    }
}
