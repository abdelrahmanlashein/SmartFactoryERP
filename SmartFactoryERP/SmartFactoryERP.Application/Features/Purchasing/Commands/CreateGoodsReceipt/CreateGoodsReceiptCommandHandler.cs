using MediatR;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateGoodsReceipt
{
    public class CreateGoodsReceiptCommandHandler : IRequestHandler<CreateGoodsReceiptCommand, int>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IInventoryRepository _inventoryRepository; // Needed to update stock
        private readonly IUnitOfWork _unitOfWork;

        public CreateGoodsReceiptCommandHandler(IPurchasingRepository purchasingRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the Aggregate Root (Purchase Order) for validation
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.PurchaseOrderId, cancellationToken);
            if (order == null)
            {
                throw new Exception($"Purchase Order {request.PurchaseOrderId} not found.");
            }
            if (order.Status != PurchaseOrderStatus.Confirmed && order.Status != PurchaseOrderStatus.PartiallyReceived)
            {
                throw new Exception($"Cannot receive goods for order status: {order.Status}.");
            }

            // 2. Create the Goods Receipt Aggregate Root
            var receipt = GoodsReceipt.Create(request.PurchaseOrderId, request.ReceivedById, request.Notes);

            // 3. Process Items and Update Stock/PO Status
            foreach (var itemDto in request.Items.Where(i => i.ReceivedQuantity > 0))
            {
                var poItem = order.Items.FirstOrDefault(i => i.Id == itemDto.POItemId);
                if (poItem == null) continue; // Item not part of the PO

                // Fetch Material Aggregate Root to update stock (Cross-Aggregate)
                var material = await _inventoryRepository.GetMaterialByIdAsync(poItem.MaterialID, cancellationToken);

                // Update Material Stock Level (Domain Logic)
                material.IncreaseStock(itemDto.ReceivedQuantity);

                // Add item to the Goods Receipt Aggregate
                receipt.AddReceivedItem(itemDto.POItemId, itemDto.ReceivedQuantity, itemDto.RejectedQuantity);
            }

            // 4. Finalize & Persist Aggregates
            // Note: In a real app, logic here checks if total received = total ordered to set status to Complete/Partial
          //  order.FinalizeReceiptStatusBasedOnItems(); // Assume this method is implemented in PurchaseOrder.cs

            await _purchasingRepository.AddGoodsReceiptAsync(receipt, cancellationToken); // Assume this is added to IPurchasingRepository

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return receipt.Id;
        }
    }
}
