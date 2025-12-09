using MediatR;

namespace SmartFactoryERP.Application.Features.UserManagement.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDetailsDto>
    {
        public string UserId { get; set; }
    }
}