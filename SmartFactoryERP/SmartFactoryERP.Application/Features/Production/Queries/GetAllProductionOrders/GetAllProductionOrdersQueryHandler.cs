using MediatR;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetAllProductionOrders
{
    public class GetAllProductionOrdersQueryHandler : IRequestHandler<GetAllProductionOrdersQuery, List<ProductionOrderDto>>
    {
        private readonly IProductionRepository _productionRepository;

        public GetAllProductionOrdersQueryHandler(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }

        public async Task<List<ProductionOrderDto>> Handle(GetAllProductionOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _productionRepository.GetAllProductionOrdersAsync(cancellationToken);

            return orders.Select(o => new ProductionOrderDto
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
        }
    }
}