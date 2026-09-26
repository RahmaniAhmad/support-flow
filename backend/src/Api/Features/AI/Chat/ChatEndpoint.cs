using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.AI.Chat;

public static class ChatEndpoint
{
    public static void MapChat(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/ai/chat",
            async (
                ChatRequest request,
                HttpContext context,
                ISender sender,
                CancellationToken cancellationToken) =>
            {

                var result =
                    await sender.Send(
                        new ChatCommand(request.Question), cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.AiSemanticSearch);
    }
}