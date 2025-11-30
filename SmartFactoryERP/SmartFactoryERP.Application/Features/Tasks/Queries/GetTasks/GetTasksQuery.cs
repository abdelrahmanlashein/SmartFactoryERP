using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQuery : IRequest<List<TaskDto>> { }
}
