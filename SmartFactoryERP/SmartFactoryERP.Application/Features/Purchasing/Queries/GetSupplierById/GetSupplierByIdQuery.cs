using MediatR;
using SmartFactoryERP.Application.Features.Purchasing.Queries.GetSuppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetSupplierById
{
    // Query returns a single SupplierDto
    public class GetSupplierByIdQuery : IRequest<SupplierDto>
    {
        public int Id { get; set; }
    }
}
