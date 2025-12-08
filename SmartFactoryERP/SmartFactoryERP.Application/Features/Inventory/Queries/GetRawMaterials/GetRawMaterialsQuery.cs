using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using System.Collections.Generic;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetRawMaterials
{
    public class GetRawMaterialsQuery : IRequest<List<MaterialDto>>
    {
        // No parameters - just get all raw materials
    }
}