using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Unit>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupplierCommandHandler(IPurchasingRepository purchasingRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch the entity from the database
            var supplierToUpdate = await _purchasingRepository.GetSupplierByIdAsync(request.Id, cancellationToken);

            if (supplierToUpdate == null)
            {
                // In a proper setup, this should throw a NotFoundException (404)
                throw new Exception($"Supplier with Id {request.Id} was not found.");
            }

            // 2. Call the Domain method to apply changes (Business Logic is contained here)
            supplierToUpdate.UpdateDetails(
                request.SupplierName,
                request.ContactPerson,
                request.Email,
                request.PhoneNumber,
                request.Address
            );

            // 3. Save changes (EF Core tracks the changes automatically)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value; // Indicates a successful void operation
        }
    }
}
