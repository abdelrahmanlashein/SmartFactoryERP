using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetAllPurchaseOrders
{
    // Query returns a list of PurchaseOrderListDto
    public class GetAllPurchaseOrdersQuery : IRequest<List<PurchaseOrderListDto>>
    {
        // Placeholders for filtering parameters if needed (e.g., Status, SupplierId)
    }
}
