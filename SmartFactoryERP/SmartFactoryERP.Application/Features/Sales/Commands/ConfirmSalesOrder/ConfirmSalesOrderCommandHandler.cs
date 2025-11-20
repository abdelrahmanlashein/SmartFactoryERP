using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Sales.Commands.ConfirmSalesOrder
{
    public class ConfirmSalesOrderCommandHandler : IRequestHandler<ConfirmSalesOrderCommand, Unit>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IInventoryRepository _inventoryRepository; // Required for stock updates
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmSalesOrderCommandHandler(
            ISalesRepository salesRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _salesRepository = salesRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ConfirmSalesOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the Sales Order (Must include Items to process stock)
            // Note: We need to ensure GetSalesOrderWithItemsAsync exists in repository
            var order = await _salesRepository.GetSalesOrderWithItemsAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new Exception($"Sales Order {request.Id} not found.");
            }

            // 2. Iterate through each item to deduct stock
            foreach (var item in order.Items)
            {
                // Fetch the Material Aggregate Root
                var material = await _inventoryRepository.GetMaterialByIdAsync(item.MaterialId, cancellationToken);

                if (material == null)
                {
                    throw new Exception($"Material with ID {item.MaterialId} not found.");
                }

                // Deduct stock (Domain Logic)
                // This will throw an exception if CurrentStock < OrderQuantity
                try
                {
                    material.DecreaseStock(item.Quantity);
                }
                catch (Exception ex)
                {
                    // Re-throw with more context about which item failed
                    throw new Exception($"Stock reservation failed for Material '{material.MaterialName}': {ex.Message}");
                }
            }

            // 3. Confirm the Order (Change Status)
            order.Confirm();

            // 4. Persist all changes (Sales Order Status + Material Stock Levels) in one transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
