using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.DeactivateSupplier
{
    public class DeactivateSupplierCommandHandler : IRequestHandler<DeactivateSupplierCommand, Unit>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateSupplierCommandHandler(IPurchasingRepository purchasingRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateSupplierCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the entity from the database
            var supplierToDeactivate = await _purchasingRepository.GetSupplierByIdAsync(request.Id, cancellationToken);

            if (supplierToDeactivate == null)
            {
                // Throw exception if not found
                throw new Exception($"Supplier with Id {request.Id} was not found.");
            }

            // 2. Call the Domain method to apply the soft delete (IsActive = false)
            supplierToDeactivate.Deactivate();

            // 3. Save changes (EF Core tracks the change on the IsActive property)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value; // Indicates a successful void operation
        }
    }
}
