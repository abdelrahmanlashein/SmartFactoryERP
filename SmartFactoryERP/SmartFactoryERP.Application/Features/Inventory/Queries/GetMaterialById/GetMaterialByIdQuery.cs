using MediatR;
using SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Queries.GetMaterialById
{
    public class GetMaterialByIdQuery : IRequest<MaterialDto>
    {
        public int Id { get; set; } 
    }
}
