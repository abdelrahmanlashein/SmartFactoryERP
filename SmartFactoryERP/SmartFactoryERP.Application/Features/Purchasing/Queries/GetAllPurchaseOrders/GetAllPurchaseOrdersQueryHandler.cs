using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetAllPurchaseOrders
{
    public class GetAllPurchaseOrdersQueryHandler : IRequestHandler<GetAllPurchaseOrdersQuery, List<PurchaseOrderListDto>>
    {
        private readonly IPurchasingRepository _purchasingRepository;

        public GetAllPurchaseOrdersQueryHandler(IPurchasingRepository purchasingRepository)
        {
            _purchasingRepository = purchasingRepository;
        }

        public async Task<List<PurchaseOrderListDto>> Handle(GetAllPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch data from Persistence layer
            var orders = await _purchasingRepository.GetAllPurchaseOrdersAsync(cancellationToken);

            // 2. Map Domain Entities to List DTOs
            return orders.Select(po => new PurchaseOrderListDto
            {
                Id = po.Id,
                PONumber = po.PONumber,
                SupplierName = po.Supplier?.SupplierName, // Safe navigation if Supplier is included
                OrderDate = po.OrderDate,
                TotalAmount = po.TotalAmount,
                Status = po.Status.ToString()
            }).ToList();
        }
    }
}
