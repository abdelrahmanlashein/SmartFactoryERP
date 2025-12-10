using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Queries.GetBomByProductId
{
    public class GetBomByProductIdQuery : IRequest<BomDto>
    {
        public int ProductId { get; set; }
    }
}
