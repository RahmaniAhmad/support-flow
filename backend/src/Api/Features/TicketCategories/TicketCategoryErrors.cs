using Microsoft.EntityFrameworkCore;
using Npgsql;
using Api.Exceptions;

namespace Api.Features.TicketCategories;

internal static class TicketCategoryErrors
{
    public const string NotFoundMessage = "Ticket category was not found.";
    public const string NotFoundCode = "TICKET_CATEGORY_NOT_FOUND";
    public const string DuplicateMessage =
        "A category with this name already exists in this company.";
    public const string DuplicateCode = "TICKET_CATEGORY_DUPLICATE";
    public const string CompanyContextRequired =
        "A company context is required.";

    public static BadRequestException MissingCompanyContext()
    {
        return new BadRequestException(
            CompanyContextRequired,
            "COMPANY_CONTEXT_REQUIRED");
    }

    public static bool IsDuplicateNameViolation(
        DbUpdateException exception)
    {
        return exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_TicketCategories_CompanyId_NormalizedName"
        };
    }
}
