using MediatR;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

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
            var order = await _productionRepository.GetProductionOrderByIdAsync(request.Id, cancellationToken);

            if (order == null)
                return null;

            return new ProductionOrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                ProductId = order.ProductId,
                ProductName = order.Product?.MaterialName ?? "Unknown",
                Quantity = order.Quantity,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                Status = order.Status.ToString(),
                Notes = order.Notes,
                CreatedDate = order.CreatedDate
            };
        }
    }
}