using MediatR;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, int>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IInventoryRepository _inventoryRepository; // Needed for future checks

        public CreatePurchaseOrderCommandHandler(IPurchasingRepository purchasingRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Business Rule: Check if the Supplier exists
            var supplier = await _purchasingRepository.GetSupplierByIdAsync(request.SupplierId, cancellationToken);
            if (supplier == null || !supplier.IsActive)
            {
                throw new Exception("Invalid or inactive supplier selected.");
            }

            // 2. Create the Order Header (The Aggregate Root)
            var poNumber = request.PONumber ?? Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            var order = PurchaseOrder.Create(
                poNumber,
                request.SupplierId,
                request.ExpectedDeliveryDate
            );

            // 3. Add Items to the Order (Delegating logic to the Aggregate Root)
            foreach (var itemDto in request.Items)
            {
                order.AddItem(itemDto.MaterialId, itemDto.Quantity, itemDto.UnitPrice);
            }

            // 4. Add the Order to the Repository
            await _purchasingRepository.AddPurchaseOrderAsync(order, cancellationToken);

            // 5. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
