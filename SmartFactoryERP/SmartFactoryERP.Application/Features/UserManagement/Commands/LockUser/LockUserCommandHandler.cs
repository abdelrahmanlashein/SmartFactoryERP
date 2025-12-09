using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Entities.Identity;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.LockUser
{
    public class LockUserCommandHandler : IRequestHandler<LockUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public LockUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            var lockoutEnd = DateTimeOffset.UtcNow.AddDays(request.LockoutDurationInDays);
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to lock user: {errors}");
            }

            return true;
        }
    }
}