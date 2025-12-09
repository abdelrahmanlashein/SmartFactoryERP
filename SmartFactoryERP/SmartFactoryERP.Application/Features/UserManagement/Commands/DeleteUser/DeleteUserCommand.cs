using MediatR;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}