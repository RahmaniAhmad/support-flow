using MediatR;

namespace Api.Features.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IReadOnlyList<GetUsersResponse>>;