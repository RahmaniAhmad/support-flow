using Api.Authorization;
using Shared.Authentication;
using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace UnitTests.Tickets;

public sealed class TicketCategoryAccessTests
{
    [Fact]
    public void Category_management_is_granted_to_admin_not_customers()
    {
        Assert.Contains(
            Permissions.TicketCategoriesManage,
            RolePermissions.Map[UserRole.Admin]);
        Assert.DoesNotContain(
            Permissions.TicketCategoriesManage,
            RolePermissions.Map[UserRole.Customer]);
        Assert.DoesNotContain(
            Permissions.TicketCategoriesManage,
            RolePermissions.Map[UserRole.Agent]);
        Assert.DoesNotContain(
            Permissions.TicketCategoriesManage,
            RolePermissions.Map[UserRole.SuperAdmin]);
    }

    [Fact]
    public void Admin_ticket_filter_excludes_other_company_tickets()
    {
        var companyId = Guid.NewGuid();
        var currentUser = new TestCurrentUser(
            Guid.NewGuid(),
            companyId,
            UserRole.Admin);
        var tickets = new[]
        {
            Ticket.Create(
                companyId,
                Guid.NewGuid(),
                "Tenant ticket",
                "Description"),
            Ticket.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Other tenant ticket",
                "Description")
        };
        var accessService = new TicketAccessService(currentUser);

        var visibleTickets = accessService
            .ApplyTicketAccessFilter(tickets.AsQueryable())
            .ToList();

        Assert.Single(visibleTickets);
        Assert.Equal(companyId, visibleTickets[0].CompanyId);
    }

    [Fact]
    public void Customer_ticket_filter_only_includes_their_tickets()
    {
        var userId = Guid.NewGuid();
        var companyId = Guid.NewGuid();
        var currentUser = new TestCurrentUser(
            userId,
            companyId,
            UserRole.Customer);
        var tickets = new[]
        {
            Ticket.Create(
                companyId,
                userId,
                "My ticket",
                "Description"),
            Ticket.Create(
                companyId,
                Guid.NewGuid(),
                "Another customer's ticket",
                "Description")
        };
        var accessService = new TicketAccessService(currentUser);

        var visibleTickets = accessService
            .ApplyTicketAccessFilter(tickets.AsQueryable())
            .ToList();

        Assert.Single(visibleTickets);
        Assert.Equal(userId, visibleTickets[0].CreatedByUserId);
    }

    private sealed record TestCurrentUser(
        Guid UserId,
        Guid? CompanyId,
        UserRole Role) : ICurrentUser
    {
        public string Email => "test@example.com";
    }
}
