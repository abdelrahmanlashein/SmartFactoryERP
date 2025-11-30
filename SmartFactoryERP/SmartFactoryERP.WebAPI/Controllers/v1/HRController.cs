using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.HR.Commands.CreateDepartment;
using SmartFactoryERP.Application.Features.HR.Commands.CreateEmployee;
using SmartFactoryERP.Application.Features.HR.Queries.GetEmployee;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HRController : BaseApiController
    {
        // POST api/v1/hr/departments
        [HttpPost("departments")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command) //tested

        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        // POST api/v1/hr/employees
        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command) //tested
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        // (مستقبلاً سنضيف GET هنا لجلب القوائم)

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees() //tested
        {
            return Ok(await Mediator.Send(new GetEmployeesQuery()));
        }
    }
}
