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

    public void AssignTo(Guid assignedByUserId, Guid assignedToUserId)
    {
        if (assignedByUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid user is required to assign the ticket.",
                nameof(assignedByUserId));
        }

        if (assignedToUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "Assigned user id is required.",
                nameof(assignedToUserId));
        }

        if (AssignedToUserId == assignedToUserId)
            throw new InvalidOperationException(
                "Ticket is already assigned to this user.");

        EnsureTransitionAllowed(TicketStatus.Assigned);

        AssignedToUserId = assignedToUserId;

        ChangeStatus(TicketStatus.Assigned);

        AddDomainEvent(
            new TicketAssignedDomainEvent(
                Id,
                CompanyId,
                assignedByUserId,
                assignedToUserId));
    }

    public void StartProgress(Guid startedByUserId)
    {
        EnsureAssignedAgent(startedByUserId);

        TransitionTo(
            TicketStatus.InProgress,
            new TicketProgressStartedDomainEvent(
                Id,
                CompanyId,
                startedByUserId));
    }

    public void MoveToPending(Guid movedToPendingByUserId)
    {
        EnsureAssignedAgent(movedToPendingByUserId);

        TransitionTo(
            TicketStatus.Pending,
            new TicketPendingDomainEvent(
                Id,
                CompanyId,
                movedToPendingByUserId));
    }

    public void Resolve(Guid resolvedByUserId)
    {
        EnsureAssignedAgent(resolvedByUserId);

        TransitionTo(
            TicketStatus.Resolved,
            new TicketResolvedDomainEvent(
                Id,
                CompanyId,
                resolvedByUserId));
    }

    public void Close(Guid closedByUserId)
    {
        if (closedByUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid user is required to close the ticket.",
                nameof(closedByUserId));
        }

        TransitionTo(
            TicketStatus.Closed,
            new TicketClosedDomainEvent(Id, CompanyId, closedByUserId));
    }

    public void Reopen(Guid reopenedByUserId)
    {
        if (reopenedByUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid user is required to reopen the ticket.",
                nameof(reopenedByUserId));
        }

        TransitionTo(
            TicketStatus.Reopened,
            new TicketReopenedDomainEvent(Id, CompanyId, reopenedByUserId));
    }

    public Guid AddComment(Guid commentedByUserId, string content)
    {
        if (commentedByUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid user is required to add a comment.",
                nameof(commentedByUserId));
        }

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Comment cannot be empty.");

        if (Status is TicketStatus.Closed)
            throw new InvalidOperationException("Cannot comment on a closed ticket.");

        var comment = TicketComment.Create
        (
             Id,
             commentedByUserId,
             content
        );

        _comments.Add(comment);

        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCommentAddedDomainEvent(
                Id,
                CompanyId,
                commentedByUserId,
                comment.Id));

        return comment.Id;
    }

    private void EnsureAssignedAgent(Guid actingUserId)
    {
        if (actingUserId == Guid.Empty)
        {
            throw new ArgumentException(
                "A valid user is required.",
                nameof(actingUserId));
        }

        if (AssignedToUserId is null)
        {
            throw new InvalidOperationException(
                "Ticket must be assigned to an agent.");
        }

        if (AssignedToUserId != actingUserId)
        {
            throw new InvalidOperationException(
                "Only the assigned agent can perform this action.");
        }
    }

    private void ChangeStatus(TicketStatus newStatus)
    {
        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void EnsureTransitionAllowed(TicketStatus newStatus)
    {
        if (!TicketWorkflow.CanTransition(Status, newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot transition ticket from {Status} to {newStatus}.");
        }
    }

    private void TransitionTo(
        TicketStatus newStatus,
        IDomainEvent domainEvent)
    {
        EnsureTransitionAllowed(newStatus);

        ChangeStatus(newStatus);

        AddDomainEvent(domainEvent);
    }
}
