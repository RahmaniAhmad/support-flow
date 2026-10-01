using MediatR;

namespace Api.Features.Users.Queries.GetUser;

public sealed record GetUserQuery(
    Guid UserId) : IRequest<GetUserResponse>;