using Shared.Domain.Base;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Tickets.Workflows;

namespace Shared.Domain.Tickets;

public sealed class Ticket : AggregateRoot
{
    public Guid CompanyId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TicketStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    private readonly List<TicketComment> _comments = [];
    public IReadOnlyCollection<TicketComment> Comments => _comments.AsReadOnly();

    private Ticket() { }
    public static Ticket Create(Guid companyId, Guid userId, string subject, string description)
    {
        var ticket = new Ticket
        {
            CompanyId = companyId,
            CreatedByUserId = userId,
            Subject = subject,
            Description = description,
            Status = TicketStatus.Open,
            CreatedAtUtc = DateTime.UtcNow
        };

        ticket.AddDomainEvent(new TicketCreatedDomainEvent(ticket.Id, ticket.CompanyId, ticket.Subject));

        return ticket;
    }

    public void AssignTo(Guid userId)
    {
        if (Status is TicketStatus.Closed or TicketStatus.Resolved)
            throw new InvalidOperationException($"Cannot assign a ticket in {Status} status.");

        AssignedToUserId = userId;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketAssignedDomainEvent(
                Id,
                userId));
    }

    public void StartProgress()
    {
        if (AssignedToUserId is null)
            throw new InvalidOperationException(
                "Cannot start progress on an unassigned ticket.");


        TransitionTo(
            TicketStatus.InProgress,
            new TicketInProgressDomainEvent(Id));
    }

    public void Resolve()
    {
        TransitionTo(
            TicketStatus.Resolved,
            new TicketResolvedDomainEvent(Id));

    }

    public void Close()
    {
        TransitionTo(
            TicketStatus.Closed,
            new TicketClosedDomainEvent(Id));
    }

    public void Reopen()
    {
        TransitionTo(
            TicketStatus.Reopened,
            new TicketReopenedDomainEvent(Id));
    }

    public Guid AddComment(Guid userId, string content)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot comment on a closed ticket.");

        var comment = new TicketComment
        {
            TicketId = Id,
            AuthorUserId = userId,
            Content = content,
            CreatedAtUtc = DateTime.UtcNow
        };

        _comments.Add(comment);

        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCommentAddedDomainEvent(
                Id,
                userId,
                content));

        return comment.Id;
    }

    private void TransitionTo(
        TicketStatus newStatus,
        IDomainEvent domainEvent)
    {
        if (!TicketWorkflow.CanTransition(
                Status,
                newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot transition ticket from {Status} to {newStatus}.");
        }

        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(domainEvent);
    }
}
