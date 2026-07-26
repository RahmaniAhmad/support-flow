using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Users;

namespace Api.Authorization;

public sealed class UserAccessService : IUserAccessService
{
    private readonly ICurrentUser _currentUser;


    public UserAccessService(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }


    public IQueryable<User> ApplyUserAccessFilter(
        IQueryable<User> users)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                users,


            UserRole.Admin =>
                users.Where(x =>
                    x.CompanyId == _currentUser.CompanyId),


            _ =>
                users.Where(x => false)
        };
    }


    public bool CanManageUser(User user)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                true,


            UserRole.Admin =>
                user.CompanyId == _currentUser.CompanyId
                &&
                user.Role != UserRole.Admin
                &&
                user.Role != UserRole.SuperAdmin,


            _ =>
                false
        };
    }


    public bool CanChangeRole(User user)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                true,


            UserRole.Admin =>
                user.CompanyId == _currentUser.CompanyId
                &&
                user.Role != UserRole.SuperAdmin
                &&
                user.Role != UserRole.Admin,


            _ =>
                false
        };
    }
}