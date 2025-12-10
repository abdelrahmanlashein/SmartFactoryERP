using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFactoryERP.Domain.Enums;

namespace SmartFactoryERP.Application.Features.Production.Commands.StartProduction
{
    public class StartProductionCommand : IRequest<Unit>
    {
        public int Id { get; set; } // Production Order ID
    }
}
