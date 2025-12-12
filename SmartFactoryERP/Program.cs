using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.HR.Commands.CreateDepartment;
using SmartFactoryERP.Application.Features.HR.Commands.CreateEmployee;
using SmartFactoryERP.Application.Features.HR.Commands.ManageAttendance;
using SmartFactoryERP.Application.Features.HR.Queries.GetDepartments;
using SmartFactoryERP.Application.Features.HR.Queries.GetEmployee;
using SmartFactoryERP.Application.Features.HR.Queries.GetTodayAttendanceQuery;
using SmartFactoryERP.Domain.Common;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] // ✅ REQUIRED - Base authentication
    public class HRController : BaseApiController
    {
        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost("departments")]
        [Authorize(Policy = "CanManageHR")] // ✅ Use policy
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { id, message = "Department created successfully" });
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        [HttpPost("employees")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { id, message = "Employee created successfully" });
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet("employees")]
        [Authorize(Policy = "CanViewHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await Mediator.Send(new GetEmployeesQuery()));
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet("departments")]
        [Authorize(Policy = "CanViewHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDepartments()
        {
            return Ok(await Mediator.Send(new GetDepartmentsQuery()));
        }

        /// <summary>
        /// Toggle attendance (check-in/check-out)
        /// </summary>
        [HttpPost("attendance/toggle")]
        [Authorize(Policy = "CanManageAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ToggleAttendance([FromBody] ToggleAttendanceCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new { message = result });
        }

        /// <summary>
        /// Get today's attendance records
        /// </summary>
        [HttpGet("attendance/today")]
        [Authorize(Policy = "CanManageAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTodayAttendance()
        {
            return Ok(await Mediator.Send(new GetTodayAttendanceQuery()));
        }
    }
}