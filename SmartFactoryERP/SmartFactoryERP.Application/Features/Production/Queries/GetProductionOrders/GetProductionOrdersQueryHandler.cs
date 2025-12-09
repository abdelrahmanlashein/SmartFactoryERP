using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders
{
    public class GetProductionOrdersQueryHandler : IRequestHandler<GetProductionOrdersQuery, List<ProductionOrderDto>>
    {
        private readonly IProductionRepository _productionRepository;

        public GetProductionOrdersQueryHandler(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }

        public async Task<List<ProductionOrderDto>> Handle(GetProductionOrdersQuery request, CancellationToken cancellationToken)
        {
            // 1. ÌáÈ ÌãíÚ ÃæÇãÑ ÇáÅäÊÇÌ
            var orders = await _productionRepository.GetAllProductionOrdersAsync(cancellationToken);

            // 2. ÝáÊÑÉ ÍÓÈ ÇáÍÇáÉ (ÅÐÇ Êã ÊÍÏíÏåÇ)
            if (!string.IsNullOrEmpty(request.Status))
            {
                orders = orders.Where(o => o.Status.ToString() == request.Status).ToList();
            }

            // 3. ÊÍæíá Åáì DTO
            var result = orders.Select(o => new ProductionOrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                ProductId = o.ProductId,
                ProductName = o.Product?.MaterialName ?? "Unknown",
                Quantity = o.Quantity,
                StartDate = o.StartDate,
                EndDate = o.EndDate,
                Status = o.Status.ToString(),
                Notes = o.Notes,
                CreatedDate = o.CreatedDate
            }).ToList();

            return result;
        }
    }
}