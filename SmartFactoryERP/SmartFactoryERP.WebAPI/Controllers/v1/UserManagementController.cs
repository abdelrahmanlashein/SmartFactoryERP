using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Identity.Models;
using SmartFactoryERP.Application.Features.UserManagement.Commands.DeleteUser;
using SmartFactoryERP.Application.Features.UserManagement.Commands.LockUser;
using SmartFactoryERP.Application.Features.UserManagement.Commands.UpdateUser;
using SmartFactoryERP.Application.Features.UserManagement.Queries.GetAllUsers;
using SmartFactoryERP.Application.Features.UserManagement.Queries.GetUserById;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Common;
using MediatR;

namespace SmartFactoryERP.WebAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    public class UserManagementController : BaseApiController
    {
        private readonly IAuthService _authService;

        public UserManagementController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: api/v1/usermanagement
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? searchTerm,
            [FromQuery] string? role,
            [FromQuery] bool? isLocked)
        {
            var query = new GetAllUsersQuery
            {
                SearchTerm = searchTerm,
                Role = role,
                IsLocked = isLocked
            };
            var users = await Mediator.Send(query);
            return Ok(users);
        }

        // GET: api/v1/usermanagement/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var query = new GetUserByIdQuery { UserId = id };
            var user = await Mediator.Send(query);
            return Ok(user);
        }

        // PUT: api/v1/usermanagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand command)
        {
            command.UserId = id;
            var result = await Mediator.Send(command);
            return Ok(new { success = result, message = "User updated successfully" });
        }

        // DELETE: api/v1/usermanagement/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)] // فقط SuperAdmin يمكنه حذف المستخدمين
        public async Task<IActionResult> DeleteUser(string id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var result = await Mediator.Send(command);
            return Ok(new { success = result, message = "User deleted successfully" });
        }

        // POST: api/v1/usermanagement/{id}/lock
        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockUser(string id, [FromBody] LockUserCommand command)
        {
            command.UserId = id;
            var result = await Mediator.Send(command);
            return Ok(new { success = result, message = "User locked successfully" });
        }

        // POST: api/v1/usermanagement/{id}/unlock
        [HttpPost("{id}/unlock")]
        public async Task<IActionResult> UnlockUser(string id)
        {
            var result = await _authService.UnlockAccount(id);
            return Ok(new { success = result, message = "User unlocked successfully" });
        }

        // POST: api/v1/usermanagement/{id}/roles/assign
        [HttpPost("{id}/roles/assign")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] AssignRoleRequest request)
        {
            request.UserId = id;
            var result = await _authService.AssignRoleToUser(request);
            return Ok(result);
        }

        // POST: api/v1/usermanagement/{id}/roles/remove
        [HttpPost("{id}/roles/remove")]
        public async Task<IActionResult> RemoveRole(string id, [FromBody] AssignRoleRequest request)
        {
            request.UserId = id;
            var result = await _authService.RemoveRoleFromUser(request);
            return Ok(result);
        }

        // GET: api/v1/usermanagement/{id}/roles
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var result = await _authService.GetUserRoles(id);
            return Ok(result);
        }

        // GET: api/v1/usermanagement/roles
        [HttpGet("roles")]
        [AllowAnonymous] // يمكن للجميع رؤية قائمة الأدوار
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _authService.GetAllRoles();
            return Ok(roles);
        }

        // GET: api/v1/usermanagement/{id}/security
        [HttpGet("{id}/security")]
        public async Task<IActionResult> GetAccountSecurity(string id)
        {
            var result = await _authService.GetAccountSecurity(id);
            return Ok(result);
        }
    }
}