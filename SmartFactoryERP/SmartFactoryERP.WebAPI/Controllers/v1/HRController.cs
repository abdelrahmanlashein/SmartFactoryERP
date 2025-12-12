using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.HR.Commands.CreateDepartment;
using SmartFactoryERP.Application.Features.HR.Commands.CreateEmployee;
using SmartFactoryERP.Application.Features.HR.Commands.DeleteDepartment;
using SmartFactoryERP.Application.Features.HR.Commands.DeleteEmployee;
using SmartFactoryERP.Application.Features.HR.Commands.ManageAttendance;
using SmartFactoryERP.Application.Features.HR.Commands.UpdateDepartment;
using SmartFactoryERP.Application.Features.HR.Commands.UpdateEmployee;
using SmartFactoryERP.Application.Features.HR.Queries.GetDepartments;
using SmartFactoryERP.Application.Features.HR.Queries.GetEmployee;
using SmartFactoryERP.Application.Features.HR.Queries.GetTodayAttendanceQuery;
using SmartFactoryERP.Domain.Common;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] // ✅ Base authentication required
    public class HRController : BaseApiController
    {
        // ===========================
        // DEPARTMENT ENDPOINTS
        // ===========================

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost("departments")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            try
            {
                var id = await Mediator.Send(command);
                return Ok(new { id, message = "Department created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        /// Update an existing department
        /// </summary>
        [HttpPut("departments/{id}")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentCommand command)
        {
            try
            {
                command.Id = id;
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "Department updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a department
        /// </summary>
        [HttpDelete("departments/{id}")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var command = new DeleteDepartmentCommand { Id = id };
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "Department deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ===========================
        // EMPLOYEE ENDPOINTS
        // ===========================

        /// <summary>
        /// Create a new employee
        /// </summary>
        [HttpPost("employees")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            try
            {
                var id = await Mediator.Send(command);
                return Ok(new { id, message = "Employee created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        /// Update an existing employee
        /// </summary>
        [HttpPut("employees/{id}")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeCommand command)
        {
            try
            {
                command.Id = id;
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        [HttpDelete("employees/{id}")]
        [Authorize(Policy = "CanManageHR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var command = new DeleteEmployeeCommand { Id = id };
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ===========================
        // ATTENDANCE ENDPOINTS
        // ===========================

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
