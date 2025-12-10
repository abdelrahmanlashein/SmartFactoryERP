using MediatR;

namespace SmartFactoryERP.Application.Features.UserManagement.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserListDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public bool? IsLocked { get; set; }
    }
}