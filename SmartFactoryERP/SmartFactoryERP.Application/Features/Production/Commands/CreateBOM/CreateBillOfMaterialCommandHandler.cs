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
        // Optionally: Inject IInventoryRepository to verify IDs exist

        public CreateBillOfMaterialCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBillOfMaterialCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Domain Entity
            // The Domain Logic inside Create() ensures Quantity > 0 and Product != Component
            var bom = BillOfMaterial.Create(
                request.ProductId,
                request.ComponentId,
                request.Quantity
            );

            // 2. Add to Repository
            await _productionRepository.AddBillOfMaterialAsync(bom, cancellationToken);

            // 3. Save Changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return bom.Id;
        }
    }
}
