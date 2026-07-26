using Shared.Domain;

namespace Api.Authorization;

public interface IUserAccessService
{
    IQueryable<User> ApplyUserAccessFilter(
        IQueryable<User> users);

    bool CanManageUser(User user);

    bool CanChangeRole(User user);
}