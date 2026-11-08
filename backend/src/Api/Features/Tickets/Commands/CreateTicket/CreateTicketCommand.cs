using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.Features.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(
    string Subject,
    string Description,
    Guid? CategoryId = null,
    IReadOnlyList<IFormFile>? Attachments = null) : IRequest<Guid>;
