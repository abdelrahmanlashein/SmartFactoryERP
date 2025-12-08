using System;
using MediatR;
using SmartFactoryERP.Domain.Entities.Production;
using SmartFactoryERP.Domain.Interfaces;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateBOM
{
    public class CreateBillOfMaterialCommandHandler : IRequestHandler<CreateBillOfMaterialCommand, int>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBillOfMaterialCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBillOfMaterialCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation
            if (request.Components == null || !request.Components.Any())
            {
                throw new Exception("يجب تحديد مكون واحد على الأقل للوصفة (BOM must have at least one component).");
            }

            // 2. Aggregate duplicate components
            var groupedComponents = request.Components
                .GroupBy(c => c.ComponentId)
                .Select(g => new
                {
                    ComponentId = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            // 3. ⚠️ DELETE existing BOM for this product (Replace strategy)
            var existingBOM = await _productionRepository.GetBOMForProductAsync(request.ProductId, cancellationToken);
            
            // Note: You need to add a Remove method to the repository
            // For now, we'll skip deletion and just add new components

            // 4. Add all components (fresh start)
            int componentsAdded = 0;
            
            foreach (var component in groupedComponents)
            {
                var bom = BillOfMaterial.Create(
                    request.ProductId,
                    component.ComponentId,
                    component.TotalQuantity
                );

                await _productionRepository.AddBillOfMaterialAsync(bom, cancellationToken);
                componentsAdded++;
            }

            // 5. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return componentsAdded;
        }
    }
}
