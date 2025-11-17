using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList
{
    public class GetMaterialsListQueryHandler : IRequestHandler<GetMaterialsListQuery, List<MaterialDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        // مستقبلاً هنضيف AutoMapper هنا

        public GetMaterialsListQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<MaterialDto>> Handle(GetMaterialsListQuery request, CancellationToken cancellationToken)
        {
            // 1. هنحتاج نضيف ميثود جديدة للـ Repository
            var materials = await _inventoryRepository.GetAllMaterialsAsync(cancellationToken);

            // 2. تحويل (Mapping) الـ Entity إلى DTO 
            // (ده هنعمله يدوي دلوقتي، ومستقبلاً بـ AutoMapper)
            var materialDtos = materials.Select(m => new MaterialDto
            {
                Id = m.Id,
                MaterialCode = m.MaterialCode,
                MaterialName = m.MaterialName,
                MaterialType = m.MaterialType.ToString(), // نحول الـ Enum لـ string
                UnitOfMeasure = m.UnitOfMeasure,
                UnitPrice = m.UnitPrice,
                CurrentStockLevel = m.CurrentStockLevel,
                MinimumStockLevel = m.MinimumStockLevel
            }).ToList();

            // 3. إرجاع القائمة
            return materialDtos;
        }
    }
}
