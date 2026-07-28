using Shared.Domain;

namespace Api.Authorization;

public interface IUserAccessService
{
    IQueryable<User> ApplyUserAccessFilter(
        IQueryable<User> users);

    bool CanManageUser(User user);
    bool CanResetPassword(User user);
    bool CanChangeStatus(User user);
}