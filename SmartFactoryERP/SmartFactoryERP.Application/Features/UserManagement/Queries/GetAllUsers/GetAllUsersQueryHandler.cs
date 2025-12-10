using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Entities.Identity;

namespace SmartFactoryERP.Application.Features.UserManagement.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserListDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // ÇáÍÕæá Úáì ßá ÇáãÓÊÎÏãíä ÃæáÇð
            var allUsers = _userManager.Users.ToList();

            // ÊØÈíÞ ÇáÝáÇÊÑ
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                allUsers = allUsers.Where(u =>
                    u.FullName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (request.IsLocked.HasValue)
            {
                if (request.IsLocked.Value)
                    allUsers = allUsers.Where(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow).ToList();
                else
                    allUsers = allUsers.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow).ToList();
            }

            var userDtos = new List<UserListDto>();

            foreach (var user in allUsers.OrderBy(u => u.FullName))
            {
                var roles = await _userManager.GetRolesAsync(user);

                // ÊØÈíÞ ÝáÊÑ ÇáÕáÇÍíÉ
                if (!string.IsNullOrWhiteSpace(request.Role) && !roles.Contains(request.Role))
                    continue;

                userDtos.Add(new UserListDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    EmployeeId = user.EmployeeId,
                    Roles = roles.ToList(),
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
                    LockoutEnd = user.LockoutEnd?.DateTime,
                    EmailConfirmed = user.EmailConfirmed,
                    AccessFailedCount = user.AccessFailedCount
                });
            }

            return userDtos;
        }
    }
}