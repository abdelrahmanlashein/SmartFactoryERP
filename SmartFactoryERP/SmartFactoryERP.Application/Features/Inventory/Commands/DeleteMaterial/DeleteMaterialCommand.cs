using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.DeleteMaterial
{
    public class DeleteMaterialCommand : IRequest<Unit> // Unit = void
    {
        public int Id { get; set; }
    }
}
