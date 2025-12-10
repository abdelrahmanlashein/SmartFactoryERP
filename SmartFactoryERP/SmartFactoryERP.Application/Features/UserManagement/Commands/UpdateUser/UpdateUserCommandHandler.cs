using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Entities.Identity;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found");

            // ÊÍÏíË ÇáÈíÇäÇÊ
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.Email; // íÝÖá Ãä íßæä UserName äÝÓ Email
            user.PhoneNumber = request.PhoneNumber;
            user.EmployeeId = request.EmployeeId;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }

            return true;
        }
    }
}