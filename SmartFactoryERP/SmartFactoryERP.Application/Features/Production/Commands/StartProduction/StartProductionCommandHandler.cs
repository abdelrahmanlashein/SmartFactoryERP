using MediatR;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq; // required for Any()

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
            // 1. Fetch the production order with its items
            // ✅✅ Using request.Id to match the command ✅✅
            var order = await _productionRepository.GetOrderWithItemsAsync(request.Id);

            if (order == null)
                throw new Exception($"Production Order {request.Id} not found.");

            // Ensure the order is in a status that allows starting
            if (order.Status != Domain.Enums.ProductionStatus.Planned)
                throw new Exception($"Cannot start order. Current status is {order.Status}.");

            // 2. Deduct raw materials from inventory based on the order's items
            if (order.Items == null || !order.Items.Any())
            {
                throw new Exception($"Order {order.OrderNumber} has no materials defined (BOM is empty). Cannot start production.");
            }

            foreach (var item in order.Items)
            {
                // Deduction happens here (if balance is insufficient, repository will throw)
                await _inventoryRepository.DeductStockAsync(item.MaterialId, item.Quantity, cancellationToken);
            }

            // 3. Change order status to "Started"
            order.StartProduction();

            // 4. Save all changes (ensures deduction and order status update are in same transaction)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}