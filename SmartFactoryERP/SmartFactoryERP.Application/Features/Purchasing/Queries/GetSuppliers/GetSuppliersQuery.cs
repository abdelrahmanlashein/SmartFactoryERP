using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetSuppliers
{
    public class GetSuppliersQuery : IRequest<List<SupplierDto>>
    {
        // No parameters needed yet (can add pagination later)
    }
}
