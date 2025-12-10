using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFactoryERP.Domain.Entities.Inventory;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetPurchaseOrderById
{
    public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto>
    {
        private readonly IPurchasingRepository _purchasingRepository;

        public GetPurchaseOrderByIdQueryHandler(IPurchasingRepository purchasingRepository)
        {
            _purchasingRepository = purchasingRepository;
        }

        public async Task<PurchaseOrderDto> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch the full aggregate root (Header + Items)
            var order = await _purchasingRepository.GetPurchaseOrderWithItemsAsync(request.Id, cancellationToken);

            if (order == null)
            {
                // Throw NotFoundException (404)
                throw new Exception($"Purchase Order with Id {request.Id} was not found.");
            }

            // 2. Map Domain Entity to DTO
            return new PurchaseOrderDto
            {
                Id = order.Id,
                PONumber = order.PONumber,
                SupplierId = order.SupplierID,
                SupplierName = order.Supplier?.SupplierName,
                OrderDate = order.OrderDate,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,

                Items = order.Items.Select(i => new PurchaseOrderItemDto
                {
                    Id = i.Id,
                    MaterialId = i.MaterialID,

                    // ✅✅ ضيف السطرين دول هنا ✅✅
                    MaterialName = i.Material?.MaterialName, // افترض أن اسم البروبرتي في الـ Entity هو MaterialName
                    MaterialCode = i.Material?.MaterialCode, // افترض أن اسم البروبرتي في الـ Entity هو MaterialCode
                    
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
    }
}
