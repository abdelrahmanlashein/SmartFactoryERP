using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Queries.GetGoodsReceiptById
{
    public class GetGoodsReceiptByIdQuery : IRequest<GoodsReceiptDto>
    {
        public int Id { get; set; }
    } 
}
