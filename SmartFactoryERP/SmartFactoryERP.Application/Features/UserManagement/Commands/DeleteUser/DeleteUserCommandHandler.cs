using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Entities.Identity;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to delete user: {errors}");
            }

            return true;
        }
    }
}