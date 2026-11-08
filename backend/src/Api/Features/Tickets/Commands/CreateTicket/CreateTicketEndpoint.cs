using Api.Authorization;
using Api.Features.Tickets.Attachments;
using Api.Filters;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Users;
using System.Text.Json;

namespace Api.Features.Tickets.Commands.CreateTicket;


public static class CreateTicketEndpoint
{
    public static IEndpointRouteBuilder MapCreateTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets",
            async (
                HttpRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                CreateTicketRequest ticketRequest;
                IReadOnlyList<IFormFile>? attachments = null;

                if (request.HasFormContentType)
                {
                    var form = await request.ReadFormAsync(cancellationToken);
                    Guid? categoryId = null;
                    var categoryValue = form["categoryId"].ToString();

                    if (!string.IsNullOrWhiteSpace(categoryValue))
                    {
                        if (!Guid.TryParse(categoryValue, out var parsedCategoryId))
                        {
                            throw new ValidationException(
                                "The selected ticket category is invalid.");
                        }

                        categoryId = parsedCategoryId;
                    }

                    ticketRequest = new CreateTicketRequest(
                        form["subject"].ToString(),
                        form["description"].ToString(),
                        categoryId);
                    attachments = form.Files.GetFiles("attachments");
                }
                else if (request.ContentType?.StartsWith(
                             "application/json",
                             StringComparison.OrdinalIgnoreCase) == true)
                {
                    try
                    {
                        ticketRequest = await request.ReadFromJsonAsync<CreateTicketRequest>(
                            cancellationToken)
                            ?? throw new ValidationException(
                                "Ticket details are required.");
                    }
                    catch (JsonException)
                    {
                        throw new ValidationException(
                            "Ticket details could not be read.");
                    }
                }
                else
                {
                    throw new ValidationException(
                        "The request must be JSON or multipart form data.");
                }

                var command = new CreateTicketCommand(
                    ticketRequest.Subject,
                    ticketRequest.Description,
                    ticketRequest.CategoryId,
                    attachments);

                var ticketId = await sender.Send(command, cancellationToken);

                return Results.Created($"/tickets/{ticketId}", new { Id = ticketId });

            })
            .DisableAntiforgery()
            .WithMetadata(new RequestSizeLimitAttribute(
                5 * TicketAttachmentValidator.MaximumFileSize + 1024 * 1024))
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsCreate);

        return app;
    }
}
