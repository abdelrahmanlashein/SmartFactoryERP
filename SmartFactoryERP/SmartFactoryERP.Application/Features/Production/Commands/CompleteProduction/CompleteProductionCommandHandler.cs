using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CompleteProduction
{
    public class CompleteProductionCommandHandler : IRequestHandler<CompleteProductionCommand, Unit>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IInventoryRepository _inventoryRepository; // Needed to increase stock
        private readonly IUnitOfWork _unitOfWork;

        public CompleteProductionCommandHandler(
            IProductionRepository productionRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CompleteProductionCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the Production Order
            var order = await _productionRepository.GetProductionOrderByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new Exception($"Production Order {request.Id} not found.");

            // 2. Fetch the Finished Good Material from Inventory
            // We need the Aggregate Root of the Material to update its stock
            var finishedProduct = await _inventoryRepository.GetMaterialByIdAsync(order.ProductId, cancellationToken);

            if (finishedProduct == null)
                throw new Exception($"Product Material ID {order.ProductId} not found in inventory.");

            // 3. Update Inventory (Increase Stock)
            // We produced 'Quantity' items, so we add them to the warehouse
            finishedProduct.IncreaseStock(order.Quantity);

            // 4. Complete the Order in Production Domain
            // This method changes status to 'Completed' and sets EndDate
            order.CompleteProduction();

            // 5. Save both changes (Production Status + Inventory Stock) in one transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
