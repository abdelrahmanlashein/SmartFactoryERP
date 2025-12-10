using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetStockTransactions
{
    public class GetStockTransactionsQueryHandler : IRequestHandler<GetStockTransactionsQuery, List<StockTransactionDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetStockTransactionsQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<StockTransactionDto>> Handle(GetStockTransactionsQuery request, CancellationToken cancellationToken)
        {
            // 1. استدعاء الميثود الجديدة من الـ Repository
            var transactions = await _inventoryRepository.GetTransactionsForMaterialAsync(request.MaterialId, cancellationToken);

            // 2. تحويل (Mapping) الـ Entities إلى DTOs
            var transactionDtos = transactions.Select(t => new StockTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                TransactionType = t.TransactionType.ToString(),
                Quantity = (int)t.Quantity, // Explicit cast from decimal to int
                ReferenceType = t.ReferenceType.ToString(),
                Notes = t.Notes
            }).ToList();

            // 3. إرجاع القائمة
            return transactionDtos;
        }
    }
}
