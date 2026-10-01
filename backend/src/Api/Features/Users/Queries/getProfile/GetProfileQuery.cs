using MediatR;

namespace Api.Features.Users.Queries.GetProfile;

public sealed record GetProfileQuery
    : IRequest<GetProfileResponse>;