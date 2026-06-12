using Shared.Domain.Base;
using Shared.DomainEvents;

namespace Shared.Domain;

public sealed class Ticket
{
    public Guid CompanyId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid? AssignedToUserId { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TicketStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

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
}