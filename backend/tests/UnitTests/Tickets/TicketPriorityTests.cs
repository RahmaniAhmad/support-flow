using Shared.Domain.Tickets;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Users;

namespace UnitTests.Tickets;

public sealed class TicketPriorityTests
{
    [Fact]
    public void New_ticket_defaults_to_normal_priority()
    {
        var ticket = CreateTicket();

        Assert.Equal(TicketPriority.Normal, ticket.Priority);
    }

    [Fact]
    public void Changing_priority_updates_the_ticket_and_emits_change_details()
    {
        var changedByUserId = Guid.NewGuid();
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        ticket.ChangePriority(TicketPriority.Critical, changedByUserId);

        Assert.Equal(TicketPriority.Critical, ticket.Priority);
        Assert.NotNull(ticket.UpdatedAtUtc);
        var priorityChangedEvent =
            Assert.IsType<TicketPriorityChangedDomainEvent>(
                Assert.Single(ticket.DomainEvents));
        Assert.Equal(TicketPriority.Normal, priorityChangedEvent.PreviousPriority);
        Assert.Equal(TicketPriority.Critical, priorityChangedEvent.NewPriority);
        Assert.Equal(changedByUserId, priorityChangedEvent.ChangedByUserId);
        Assert.Equal(ticket.UpdatedAtUtc, priorityChangedEvent.ChangedAtUtc);
    }

    [Fact]
    public void Changing_to_an_invalid_priority_is_rejected()
    {
        var ticket = CreateTicket();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => ticket.ChangePriority((TicketPriority)99, Guid.NewGuid()));
    }

    [Fact]
    public void Priority_management_is_granted_to_staff_but_not_customers()
    {
        Assert.Contains(
            Permissions.TicketsManagePriority,
            RolePermissions.Map[UserRole.SuperAdmin]);
        Assert.Contains(
            Permissions.TicketsManagePriority,
            RolePermissions.Map[UserRole.Admin]);
        Assert.Contains(
            Permissions.TicketsManagePriority,
            RolePermissions.Map[UserRole.Agent]);
        Assert.DoesNotContain(
            Permissions.TicketsManagePriority,
            RolePermissions.Map[UserRole.Customer]);
    }

    private static Ticket CreateTicket() =>
        Ticket.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Subject",
            "Description");
}
