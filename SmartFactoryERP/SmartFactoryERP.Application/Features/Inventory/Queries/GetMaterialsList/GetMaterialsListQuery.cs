using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList
{
    public class GetMaterialsListQuery : IRequest<List<MaterialDto>>
    {
        // ممكن نسيبه فاضي لو هنجيب كله
        // أو نضيف (int PageNumber, int PageSize)
    }
}
