using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFactoryERP.Application.Features.Identity.Models;
using SmartFactoryERP.Application.Features.UserManagement.Commands.DeleteUser;
using SmartFactoryERP.Application.Features.UserManagement.Commands.LockUser;
using SmartFactoryERP.Application.Features.UserManagement.Commands.RegisterUser;
using SmartFactoryERP.Application.Features.UserManagement.Commands.UpdateUser;
using SmartFactoryERP.Application.Features.UserManagement.Queries.GetAllUsers;
using SmartFactoryERP.Application.Features.UserManagement.Queries.GetUserById;
using SmartFactoryERP.Application.Interfaces.Identity;
using SmartFactoryERP.Domain.Common;

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

        /// <summary>
        /// Register a new user (Admin/SuperAdmin only)
        /// </summary>
        /// <param name="command">User registration details</param>
        /// <returns>Created user information</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return CreatedAtAction(
                    nameof(GetUserById), 
                    new { id = result.UserId }, 
                    result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all users with optional filtering
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<UserListDto>>> GetAllUsers(
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

        /// <summary>
        /// Get user details by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailsDto>> GetUserById(string id)
        {
            try
            {
                var query = new GetUserByIdQuery { UserId = id };
                var user = await Mediator.Send(query);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update user information
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand command)
        {
            if (command == null)
                return BadRequest(new { message = "Invalid request body" });

            command.UserId = id;
            
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete user (SuperAdmin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var command = new DeleteUserCommand { UserId = id };
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lock user account
        /// </summary>
        [HttpPost("{id}/lock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LockUser(string id, [FromBody] LockUserCommand? command)
        {
            try
            {
                command ??= new LockUserCommand();
                command.UserId = id;
                var result = await Mediator.Send(command);
                return Ok(new { success = result, message = "User locked successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Unlock user account
        /// </summary>
        [HttpPost("{id}/unlock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnlockUser(string id)
        {
            try
            {
                var result = await _authService.UnlockAccount(id);
                return Ok(new { success = result, message = "User unlocked successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Assign role to user
        /// </summary>
        [HttpPost("{id}/roles/assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserRolesResponse>> AssignRole(string id, [FromBody] AssignRoleRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request body" });

            request.UserId = id;
            
            try
            {
                var result = await _authService.AssignRoleToUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove role from user
        /// </summary>
        [HttpPost("{id}/roles/remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserRolesResponse>> RemoveRole(string id, [FromBody] AssignRoleRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request body" });

            request.UserId = id;
            
            try
            {
                var result = await _authService.RemoveRoleFromUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get user roles
        /// </summary>
        [HttpGet("{id}/roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserRolesResponse>> GetUserRoles(string id)
        {
            try
            {
                var result = await _authService.GetUserRoles(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all available roles
        /// </summary>
        [HttpGet("roles")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> GetAllRoles()
        {
            var roles = await _authService.GetAllRoles();
            return Ok(roles);
        }

        /// <summary>
        /// Get account security information
        /// </summary>
        [HttpGet("{id}/security")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountSecurity(string id)
        {
            try
            {
                var result = await _authService.GetAccountSecurity(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}