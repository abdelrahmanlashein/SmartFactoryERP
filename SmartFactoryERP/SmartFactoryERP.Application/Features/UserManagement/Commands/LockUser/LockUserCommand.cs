using MediatR;

namespace SmartFactoryERP.Application.Features.UserManagement.Commands.LockUser
{
    public class LockUserCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public int LockoutDurationInDays { get; set; } = 30; // ÇÝÊÑÇÖíÇð 30 íæã
    }
}