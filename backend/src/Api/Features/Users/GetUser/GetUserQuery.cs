using MediatR;

namespace Api.Features.Users.GetUser;

public sealed record GetUserQuery(
    Guid UserId) : IRequest<GetUserResponse>;