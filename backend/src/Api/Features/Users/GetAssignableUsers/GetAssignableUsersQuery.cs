using MediatR;

namespace Api.Features.Users.GetAssignableUsers;

public sealed record GetAssignableUsersQuery()
    : IRequest<List<GetAssignableUsersResponse>>;