using MediatR;

namespace Api.Features.AI.Chat;

public sealed record ChatCommand(
    string Question)
    : IRequest<ChatResponse>;