using MediatR;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders; // ✅ عشان يشوف الـ DTO
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Linq; // ✅ ضروري عشان دالة Select تشتغل
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrderById
{
    public class GetProductionOrderByIdQueryHandler : IRequestHandler<GetProductionOrderByIdQuery, ProductionOrderDto>
    {
        private readonly IProductionRepository _productionRepository;

        public GetProductionOrderByIdQueryHandler(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }

        public async Task<ProductionOrderDto> Handle(GetProductionOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // ✅ 1. بننادي الدالة اللي بتجيب الأوردر + الخامات (Items)
            // هام: تأكد إن الدالة دي في الريبوزيتوري بتعمل .Include(x => x.Items)
            // لو الدالة عندك اسمها GetProductionOrderByIdAsync، تأكد إنك ضفت الـ Include جواها
            var order = await _productionRepository.GetOrderWithItemsAsync(request.Id);

            if (order == null)
                return null;

            return new ProductionOrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                ProductId = order.ProductId,
                ProductName = order.Product?.MaterialName ?? "Unknown Product",
                Quantity = order.Quantity,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                Status = order.Status.ToString(),
                Notes = order.Notes,
                CreatedDate = order.CreatedDate,
                Priority = order.Priority, // ✅ الأولوية تظهر في التفاصيل

                // ✅✅ 2. الجزء الأهم: تحويل قائمة الخامات (Items) من الـ Entity للـ DTO ✅✅
                // ده الكود اللي كان ناقص وعشان كدا الجدول كان فاضي
                Items = order.Items?.Select(i => new ProductionOrderItemDto
                {
                    Id = i.Id,
                    MaterialName = i.Material?.MaterialName ?? "Unknown Material",
                    Quantity = i.Quantity,
                    Unit = i.Material?.UnitOfMeasure ?? "Unit"
                }).ToList() ?? new List<ProductionOrderItemDto>()
            };
        }
    }
}