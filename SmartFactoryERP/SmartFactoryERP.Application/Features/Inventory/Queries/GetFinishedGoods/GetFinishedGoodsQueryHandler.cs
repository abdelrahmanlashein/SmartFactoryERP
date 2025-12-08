using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetFinishedGoods
{
    public class GetFinishedGoodsQueryHandler : IRequestHandler<GetFinishedGoodsQuery, List<MaterialDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetFinishedGoodsQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<MaterialDto>> Handle(GetFinishedGoodsQuery request, CancellationToken cancellationToken)
        {
            // 1. Get all materials
            var materials = await _inventoryRepository.GetAllMaterialsAsync(cancellationToken);

            // 2. Filter by MaterialType.FinishedGood and map to DTO
            var finishedGoodDtos = materials
                .Where(m => m.MaterialType == MaterialType.FinishedGood) // ?? Filter
                .Select(m => new MaterialDto
                {
                    Id = m.Id,
                    MaterialCode = m.MaterialCode,
                    MaterialName = m.MaterialName,
                    MaterialType = m.MaterialType.ToString(),
                    UnitOfMeasure = m.UnitOfMeasure,
                    UnitPrice = m.UnitPrice,
                    CurrentStockLevel = m.CurrentStockLevel,
                    MinimumStockLevel = m.MinimumStockLevel
                }).ToList();

            return finishedGoodDtos;
        }
    }
}