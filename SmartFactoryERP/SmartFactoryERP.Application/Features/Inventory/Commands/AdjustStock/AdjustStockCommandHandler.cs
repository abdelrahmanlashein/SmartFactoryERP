using MediatR;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.AdjustStock
{
    public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, Unit>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdjustStockCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
        {
            // 1. Retrieve the entity (material)
            var material = await _inventoryRepository.GetMaterialByIdAsync(request.MaterialId, cancellationToken);
            if (material == null)
            {
                throw new Exception($"Material with Id {request.MaterialId} was not found.");
            }

            // 2. Call domain behavior to adjust the stock
            if (request.Quantity > 0)
            {
                material.IncreaseStock(request.Quantity);
            }
            else
            {
                // request.Quantity is negative here, convert to positive for deduction
                material.DecreaseStock(Math.Abs(request.Quantity));
            }

            // 3. Create a stock transaction to record the operation
            var transaction = StockTransaction.Create(
                request.MaterialId,
                TransactionType.Adjustment, // transaction type: adjustment
                request.Quantity,
                ReferenceType.ManualAdjustment, // reference: manual adjustment
                null, // no reference id
                request.Notes
            );

            // 4. Add the transaction to the repository
            await _inventoryRepository.AddStockTransactionAsync(transaction, cancellationToken);

            // 5. Save all changes (stock change + transaction) in a single transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
