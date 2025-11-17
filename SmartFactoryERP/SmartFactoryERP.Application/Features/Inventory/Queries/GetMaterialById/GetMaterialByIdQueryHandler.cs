using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialById
{
    public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto>
    {
        private readonly IInventoryRepository _inventoryRepository;
        // مستقبلاً: private readonly IMapper _mapper;

        public GetMaterialByIdQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<MaterialDto> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. استدعاء الميثود من الـ Repository
            var material = await _inventoryRepository.GetMaterialByIdAsync(request.Id, cancellationToken);

            // 2. التحقق إذا كانت المادة موجودة
            if (material == null)
            {
                // ارمي Exception مخصص
                // throw new NotFoundException(nameof(Material), request.Id);
                throw new Exception($"Material with Id {request.Id} was not found."); // مؤقتاً
            }

            // 3. تحويل (Mapping) الـ Entity إلى DTO (يدوي مؤقتاً)
            var materialDto = new MaterialDto
            {
                Id = material.Id,
                MaterialCode = material.MaterialCode,
                MaterialName = material.MaterialName,
                MaterialType = material.MaterialType.ToString(),
                UnitOfMeasure = material.UnitOfMeasure,
                UnitPrice = material.UnitPrice,
                CurrentStockLevel = material.CurrentStockLevel,
                MinimumStockLevel = material.MinimumStockLevel
            };

            // 4. إرجاع الـ DTO
            return materialDto;
        }
    }
}
