using MediatR;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetSuppliers;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetSupplierById
{
    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
    {
        private readonly IPurchasingRepository _purchasingRepository;

        public GetSupplierByIdQueryHandler(IPurchasingRepository purchasingRepository)
        {
            _purchasingRepository = purchasingRepository;
        }

        public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch data from Persistence layer
            var supplier = await _purchasingRepository.GetSupplierByIdAsync(request.Id, cancellationToken);

            // 2. Check for existence (Critical)
            if (supplier == null)
            {
                // In a proper setup, this should throw a NotFoundException (404)
                throw new Exception($"Supplier with Id {request.Id} was not found.");
            }

            // 3. Map Domain Entity to DTO (Manual mapping for now)
            return new SupplierDto
            {
                Id = supplier.Id,
                SupplierCode = supplier.SupplierCode,
                SupplierName = supplier.SupplierName,
                ContactPerson = supplier.ContactPerson,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                IsActive = supplier.IsActive
            };
        }
    }
}
