using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Tasks.Commands.CompleteTask;
using SmartFactoryERP.Application.Features.Tasks.Commands.CreateTask;
using SmartFactoryERP.Application.Features.Tasks.Commands.UpdateTaskStatus;
using SmartFactoryERP.Application.Features.Tasks.Queries.GetPerformance;
using SmartFactoryERP.Application.Features.Tasks.Queries.GetTasks;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command) //test
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks() //tested
        {
            return Ok(await Mediator.Send(new GetTasksQuery()));
        }

        [HttpGet("performance")]
        public async Task<IActionResult> GetPerformance([FromQuery] int? employeeId) // tested
        {
            // لو بعت employeeId هيجيب واحد، لو مبعتش هيجيب تقرير شامل لكل الموظفين
            return Ok(await Mediator.Send(new GetPerformanceQuery { EmployeeId = employeeId }));
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id) // tested
        {
            await Mediator.Send(new CompleteTaskCommand { Id = id });
            return NoContent();
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            // status: "Start" or "Complete"
            await Mediator.Send(new ChangeTaskStatusCommand { Id = id, NewStatus = status });
            return NoContent();
        }
    }
}
