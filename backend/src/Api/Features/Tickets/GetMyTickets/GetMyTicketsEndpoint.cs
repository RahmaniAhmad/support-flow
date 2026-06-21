using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.GetMyTickets;

public static class GetMyTicketsEndpoint
{
    public static IEndpointRouteBuilder MapGetMyTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/my",
            async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetMyTicketsQuery(currentUser.CompanyId,
                        currentUser.UserId),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization();

        return app;
    }
}
