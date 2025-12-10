using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Entities.Identity;
using SmartFactoryERP.Domain.Common;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception($"User with email {request.Email} already exists.");
            }

            // Create new user
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                EmployeeId = request.EmployeeId,
                EmailConfirmed = true // Admin-created users are auto-confirmed
            };

            // Create user with password
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            // Assign roles
            if (request.Roles != null && request.Roles.Any())
            {
                // Validate roles exist
                var validRoles = request.Roles.Where(r => Roles.AllRoles.Contains(r)).ToList();
                
                if (validRoles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, validRoles);
                    if (!roleResult.Succeeded)
                    {
                        // Log warning but don't fail - user is created
                        var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        // Consider logging: roleErrors
                    }
                }
            }
            else
            {
                // Default role if none specified
                await _userManager.AddToRoleAsync(user, Roles.Employee);
            }

            // Get assigned roles
            var assignedRoles = await _userManager.GetRolesAsync(user);

            return new RegisterUserResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                EmployeeId = user.EmployeeId,
                Roles = assignedRoles.ToList(),
                IsSuccess = true,
                Message = "User created successfully"
            };
        }
    }
}