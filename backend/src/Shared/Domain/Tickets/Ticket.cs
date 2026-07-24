using Shared.Domain.Base;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Tickets.Workflows;

namespace Shared.Domain.Tickets;

public sealed class Ticket : AggregateRoot
{
    public long TicketNumber { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TicketStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    private readonly List<TicketComment> _comments = [];
    public IReadOnlyCollection<TicketComment> Comments => _comments;

    private Ticket() { }
    public static Ticket Create(Guid companyId, Guid userId, string subject, string description)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException(
                "Company id is required.",
                nameof(companyId));

        if (userId == Guid.Empty)
            throw new ArgumentException(
                "Creator user id is required.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException(
                "Subject is required.",
                nameof(subject));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.",
                nameof(description));

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

    public void AssignTicketNumber(long ticketNumber)
    {
        if (ticketNumber <= 0)
            throw new ArgumentException(
                "Ticket number must be greater than zero.",
                nameof(ticketNumber));


        if (TicketNumber != 0)
            throw new InvalidOperationException(
                "Ticket number has already been assigned.");


        TicketNumber = ticketNumber;
    }

    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned user id is required.",
                nameof(userId));
        }

        if (Status is TicketStatus.Closed or TicketStatus.Resolved)
            throw new InvalidOperationException(
                $"Cannot assign a ticket in {Status} status.");

        if (AssignedToUserId is not null)
            throw new InvalidOperationException(
                "Ticket is already assigned.");

        AssignedToUserId = userId;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketAssignedDomainEvent(
                Id,
                CompanyId,
                userId));
    }

    public void StartProgress()
    {
        if (AssignedToUserId is null)
            throw new InvalidOperationException(
                "Cannot start progress on an unassigned ticket.");


        TransitionTo(
            TicketStatus.InProgress,
            new TicketInProgressDomainEvent(Id, CompanyId, AssignedToUserId.Value));
    }

    public void Resolve()
    {
        if (AssignedToUserId is null)
            throw new InvalidOperationException(
                "Only assigned tickets can be resolved.");

        TransitionTo(
            TicketStatus.Resolved,
            new TicketResolvedDomainEvent(Id, CompanyId, AssignedToUserId.Value));

    }

    public void Close(Guid closedByUserId)
    {
        TransitionTo(
            TicketStatus.Closed,
            new TicketClosedDomainEvent(Id, CompanyId, closedByUserId));
    }

    public void Reopen(Guid reopenedByUserId)
    {
        TransitionTo(
            TicketStatus.Reopened,
            new TicketReopenedDomainEvent(Id, CompanyId, reopenedByUserId));
    }

    public Guid AddComment(Guid userId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Comment cannot be empty.");

        if (Status is TicketStatus.Closed)
            throw new InvalidOperationException("Cannot comment on a closed ticket.");

        var comment = TicketComment.Create
        (
             Id,
             userId,
             content
        );

        _comments.Add(comment);

        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCommentAddedDomainEvent(
                Id,
                CompanyId,
                userId,
                comment.Id));

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
