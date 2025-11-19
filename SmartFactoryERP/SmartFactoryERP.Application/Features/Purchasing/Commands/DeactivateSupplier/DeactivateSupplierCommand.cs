using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.DeactivateSupplier
{
    // Command to perform a soft delete (deactivate) on a supplier
    public class DeactivateSupplierCommand : IRequest<Unit>
    {
        public int Id { get; set; } // ID المورد المراد تعطيله
    }
}
