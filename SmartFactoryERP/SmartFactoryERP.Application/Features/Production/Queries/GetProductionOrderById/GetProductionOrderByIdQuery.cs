using MediatR;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrderById
{
    public class GetProductionOrderByIdQuery : IRequest<ProductionOrderDto>
    {
        public int Id { get; set; }
    }
}