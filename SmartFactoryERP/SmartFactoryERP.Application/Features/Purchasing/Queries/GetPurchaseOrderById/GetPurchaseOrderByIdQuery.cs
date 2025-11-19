using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetPurchaseOrderById
{
    // Query returns a single PurchaseOrderDto
    public class GetPurchaseOrderByIdQuery : IRequest<PurchaseOrderDto>
    {
        public int Id { get; set; }
    }
}
