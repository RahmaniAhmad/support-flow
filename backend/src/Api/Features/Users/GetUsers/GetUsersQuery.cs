using MediatR;

namespace Api.Features.Users.GetUsers;

public record GetUsersQuery : IRequest<IReadOnlyList<GetUsersResponse>>;