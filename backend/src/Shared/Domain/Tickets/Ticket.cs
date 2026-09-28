using Shared.Domain.Base;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Tickets.Exceptions;
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

    private Ticket()
    {
    }

    public static Ticket Create(
        Guid companyId,
        Guid userId,
        string subject,
        string description)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            companyId,
            Guid.Empty);

        ArgumentOutOfRangeException.ThrowIfEqual(
            userId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            subject);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            description);

        var ticket = new Ticket
        {
            CompanyId = companyId,
            CreatedByUserId = userId,
            Subject = subject.Trim(),
            Description = description.Trim(),
            Status = TicketStatus.Open,
            CreatedAtUtc = DateTime.UtcNow
        };

        ticket.AddDomainEvent(
            new TicketCreatedDomainEvent(
                ticket.Id,
                ticket.CompanyId,
                ticket.Subject));

        return ticket;
    }

    public void AssignTicketNumber(long ticketNumber)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketNumber);

        if (TicketNumber != 0)
        {
            throw new TicketNumberAlreadyAssignedException(
                TicketNumber);
        }

        TicketNumber = ticketNumber;
    }

    public void AssignTo(
        Guid assignedByUserId,
        Guid assignedToUserId)
    {
        ValidateUserId(assignedByUserId);

        ValidateUserId(assignedToUserId);

        if (AssignedToUserId == assignedToUserId)
        {
            throw new TicketAlreadyAssignedException(
                assignedToUserId);
        }

        EnsureTransitionAllowed(
            TicketStatus.Assigned);

        var oldStatus = Status;

        AssignedToUserId = assignedToUserId;

        ChangeStatus(TicketStatus.Assigned);

        AddDomainEvent(
            new TicketAssignedDomainEvent(
                Id,
                CompanyId,
                assignedByUserId,
                assignedToUserId));

        AddDomainEvent(
            new TicketStatusChangedDomainEvent(
                Id,
                CompanyId,
                assignedByUserId,
                oldStatus,
                Status));
    }

    public void StartProgress(Guid startedByUserId)
    {
        EnsureAssignedAgent(startedByUserId);

        TransitionTo(
            TicketStatus.InProgress,
            new TicketProgressStartedDomainEvent(
                Id,
                CompanyId,
                startedByUserId),
                startedByUserId);
    }

    public void MoveToPending(Guid movedToPendingByUserId)
    {
        EnsureAssignedAgent(movedToPendingByUserId);

        TransitionTo(
            TicketStatus.Pending,
            new TicketPendingDomainEvent(
                Id,
                CompanyId,
                movedToPendingByUserId),
                movedToPendingByUserId);
    }

    public void Resolve(Guid resolvedByUserId)
    {
        EnsureAssignedAgent(resolvedByUserId);

        TransitionTo(
            TicketStatus.Resolved,
            new TicketResolvedDomainEvent(
                Id,
                CompanyId,
                resolvedByUserId),
                resolvedByUserId);
    }

    public void Close(Guid closedByUserId)
    {
        ValidateUserId(closedByUserId);

        TransitionTo(
            TicketStatus.Closed,
            new TicketClosedDomainEvent(
                Id,
                CompanyId,
                closedByUserId),
                closedByUserId);
    }

    public void Reopen(Guid reopenedByUserId)
    {
        ValidateUserId(reopenedByUserId);

        TransitionTo(
            TicketStatus.Reopened,
            new TicketReopenedDomainEvent(
                Id,
                CompanyId,
                reopenedByUserId),
                reopenedByUserId);
    }

    public Guid AddComment(
        Guid commentedByUserId,
        string content)
    {
        ValidateUserId(commentedByUserId);

        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        if (Status == TicketStatus.Closed)
        {
            throw new TicketClosedException();
        }

        var comment = TicketComment.Create(
            Id,
            commentedByUserId,
            content.Trim());

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

    private void EnsureAssignedAgent(
        Guid actingUserId)
    {
        ValidateUserId(actingUserId);

        if (AssignedToUserId is null)
        {
            throw new TicketNotAssignedException();
        }

        if (AssignedToUserId != actingUserId)
        {
            throw new NotAssignedAgentException();
        }
    }

    private void EnsureTransitionAllowed(
        TicketStatus targetStatus)
    {
        if (TicketWorkflow.CanTransition(
                Status,
                targetStatus))
        {
            return;
        }

        throw new InvalidTicketTransitionException(
            Status,
            targetStatus);
    }

    private void TransitionTo(
        TicketStatus newStatus,
        IDomainEvent domainEvent,
        Guid changedByUserId)
    {
        EnsureTransitionAllowed(newStatus);

        var oldStatus = Status;

        ChangeStatus(newStatus);

        AddDomainEvent(domainEvent);

        AddDomainEvent(
            new TicketStatusChangedDomainEvent(
                Id,
                CompanyId,
                changedByUserId,
                oldStatus,
                newStatus));
    }

    private void ChangeStatus(
        TicketStatus newStatus)
    {
        Status = newStatus;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static void ValidateUserId(Guid userId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            userId,
            Guid.Empty);
    }
}