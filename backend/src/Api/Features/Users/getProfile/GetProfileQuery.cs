using MediatR;

namespace Api.Features.Users.GetProfile;

public sealed record GetProfileQuery
    : IRequest<GetProfileResponse>;