using Api.Features.TicketCategories.Commands.CreateTicketCategory;
using Api.Features.TicketCategories.Commands.UpdateTicketCategory;

namespace UnitTests.Tickets;

public sealed class TicketCategoryValidationTests
{
    [Fact]
    public void Create_category_rejects_missing_or_overlong_names()
    {
        var validator = new CreateTicketCategoryCommandValidator();

        Assert.False(
            validator.Validate(
                new CreateTicketCategoryCommand("", null)).IsValid);
        Assert.False(
            validator.Validate(
                new CreateTicketCategoryCommand(
                    new string('a', 101),
                    null)).IsValid);
        Assert.True(
            validator.Validate(
                new CreateTicketCategoryCommand("Billing", null)).IsValid);
    }

    [Fact]
    public void Update_category_rejects_invalid_id_and_description_length()
    {
        var validator = new UpdateTicketCategoryCommandValidator();

        Assert.False(
            validator.Validate(
                new UpdateTicketCategoryCommand(
                    Guid.Empty,
                    "Billing",
                    null)).IsValid);
        Assert.False(
            validator.Validate(
                new UpdateTicketCategoryCommand(
                    Guid.NewGuid(),
                    "Billing",
                    new string('a', 501))).IsValid);
    }
}
