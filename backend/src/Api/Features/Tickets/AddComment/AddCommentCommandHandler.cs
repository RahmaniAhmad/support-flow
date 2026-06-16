using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.AddComment;


public sealed class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommand, Guid>
{
    private readonly SupportFlowDbContext _db;

    public AddCommentCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        AddCommentCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.TicketId &&
                    x.CompanyId == request.CompanyId,
                cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        var commentId = ticket.AddComment(
            request.UserId,
            request.Content);


        await _db.SaveChangesAsync(cancellationToken);

        return commentId;
    }
}
