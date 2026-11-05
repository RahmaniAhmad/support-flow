namespace Api.Features.TicketCategories;

internal static class TicketCategoryErrors
{
    public const string NotFoundMessage =
        "Ticket category was not found.";

    public const string NotFoundCode =
        "TICKET_CATEGORY_NOT_FOUND";

    public const string DuplicateMessage =
        "A category with this name already exists in this company.";

    public const string DuplicateCode =
        "TICKET_CATEGORY_DUPLICATE";
}