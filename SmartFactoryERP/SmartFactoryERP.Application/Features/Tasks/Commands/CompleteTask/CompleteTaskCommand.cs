using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommand : IRequest<Unit>
    {
        public int Id { get; set; } // رقم المهمة
    }
}
