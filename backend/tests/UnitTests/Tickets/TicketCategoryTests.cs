using Shared.Domain.Tickets;

namespace UnitTests.Tickets;

public sealed class TicketCategoryTests
{
    [Fact]
    public void Create_trims_name_and_description_and_starts_active()
    {
        var companyId = Guid.NewGuid();

        var category = TicketCategory.Create(
            companyId,
            "  Billing  ",
            "  Invoice questions  ");

        Assert.Equal(companyId, category.CompanyId);
        Assert.Equal("Billing", category.Name);
        Assert.Equal("Invoice questions", category.Description);
        Assert.True(category.IsActive);
        Assert.NotEqual(default, category.CreatedAtUtc);
    }

    [Fact]
    public void Category_can_be_deactivated_and_activated()
    {
        var category = TicketCategory.Create(Guid.NewGuid(), "Billing");

        category.Deactivate();
        Assert.False(category.IsActive);

        category.Activate();
        Assert.True(category.IsActive);
    }

    [Fact]
    public void Ticket_can_be_created_without_category_and_later_assigned_or_cleared()
    {
        var ticket = Ticket.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Subject",
            "Description");
        var categoryId = Guid.NewGuid();

        Assert.Null(ticket.CategoryId);

        ticket.AssignCategory(categoryId);
        Assert.Equal(categoryId, ticket.CategoryId);

        ticket.RemoveCategory();
        Assert.Null(ticket.CategoryId);
    }

    [Fact]
    public void Ticket_rejects_an_empty_category_id()
    {
        var ticket = Ticket.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Subject",
            "Description");

        Assert.Throws<ArgumentOutOfRangeException>(
            () => ticket.AssignCategory(Guid.Empty));
    }
}
