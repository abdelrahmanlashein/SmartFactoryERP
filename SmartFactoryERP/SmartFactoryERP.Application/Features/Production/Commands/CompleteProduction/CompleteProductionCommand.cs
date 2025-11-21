using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CompleteProduction
{
    public class CompleteProductionCommand : IRequest<Unit>
    {
        public int Id { get; set; } // Production Order ID
    }
}
