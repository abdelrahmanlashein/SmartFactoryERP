using MediatR;
using SmartFactoryERP.Application.Features.Production.Queries.GetProductionOrders;
using System.Collections.Generic;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetAllProductionOrders
{
    public class GetAllProductionOrdersQuery : IRequest<List<ProductionOrderDto>>
    {
    }
}