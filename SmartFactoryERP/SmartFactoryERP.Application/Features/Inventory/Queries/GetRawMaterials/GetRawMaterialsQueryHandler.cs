using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetRawMaterials
{
    public class GetRawMaterialsQueryHandler : IRequestHandler<GetRawMaterialsQuery, List<MaterialDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetRawMaterialsQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<MaterialDto>> Handle(GetRawMaterialsQuery request, CancellationToken cancellationToken)
        {
            // 1. Get all materials
            var materials = await _inventoryRepository.GetAllMaterialsAsync(cancellationToken);

            // 2. Filter by MaterialType.RawMaterial and map to DTO
            var rawMaterialDtos = materials
                .Where(m => m.MaterialType == MaterialType.RawMaterial) // ?? Filter
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

            return rawMaterialDtos;
        }
    }
}