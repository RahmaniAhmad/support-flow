using MediatR;

namespace Api.Features.Users.Queries.GetAssignableUsers;

public sealed record GetAssignableUsersQuery()
    : IRequest<List<GetAssignableUsersResponse>>;