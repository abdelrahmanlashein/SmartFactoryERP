using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetSuppliers
{
    public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierDto>>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        // In a real project, use AutoMapper here instead of manual mapping

        public GetSuppliersQueryHandler(IPurchasingRepository purchasingRepository)
        {
            _purchasingRepository = purchasingRepository;
        }

        public async Task<List<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch data from Persistence layer
            var suppliers = await _purchasingRepository.GetAllSuppliersAsync(cancellationToken);

            // 2. Map Domain Entities to DTOs
            return suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierCode = s.SupplierCode,
                SupplierName = s.SupplierName,
                ContactPerson = s.ContactPerson,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                IsActive = s.IsActive
            }).ToList();
        }
    }
}
