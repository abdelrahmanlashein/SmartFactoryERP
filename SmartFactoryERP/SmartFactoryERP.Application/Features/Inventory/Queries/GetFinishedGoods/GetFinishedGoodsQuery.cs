using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using System.Collections.Generic;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetFinishedGoods
{
    public class GetFinishedGoodsQuery : IRequest<List<MaterialDto>>
    {
        // No parameters - just get all finished goods
    }
}