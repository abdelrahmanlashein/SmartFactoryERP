using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetGoodsReceiptById
{
    public class GetGoodsReceiptByIdQueryHandler : IRequestHandler<GetGoodsReceiptByIdQuery, GoodsReceiptDto>
    {
        private readonly IPurchasingRepository _purchasingRepository;

        public GetGoodsReceiptByIdQueryHandler(IPurchasingRepository purchasingRepository)
        {
            _purchasingRepository = purchasingRepository;
        }

        public async Task<GoodsReceiptDto> Handle(GetGoodsReceiptByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch the full Goods Receipt Aggregate Root
            var receipt = await _purchasingRepository.GetGoodsReceiptWithItemsAsync(request.Id, cancellationToken);

            if (receipt == null)
            {
                // Throw NotFoundException (404)
                throw new Exception($"Goods Receipt with Id {request.Id} was not found.");
            }

            // 2. Map Domain Entity to DTO
            return new GoodsReceiptDto
            {
                Id = receipt.Id,
                PurchaseOrderId = receipt.PurchaseOrderID,
                ReceiptDate = receipt.ReceiptDate,
                ReceivedById = receipt.ReceivedById,
                Notes = receipt.Notes,
                Status = receipt.Status.ToString(),

                Items = receipt.Items.Select(i => new GoodsReceiptItemDto
                {
                    PoItemId = i.POItemID,
                    ReceivedQuantity = i.ReceivedQuantity,
                    RejectedQuantity = i.RejectedQuantity
                }).ToList()
            };
        }
    }
}
