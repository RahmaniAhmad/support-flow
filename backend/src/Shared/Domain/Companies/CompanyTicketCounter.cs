using Shared.Domain.Base;

namespace Shared.Domain.Companies;

public sealed class CompanyTicketCounter : Entity
{
    public Guid CompanyId { get; private set; }

    public long LastTicketNumber { get; private set; }


    private CompanyTicketCounter()
    {
    }


    public static CompanyTicketCounter Create(Guid companyId)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException(
                "Company id is required.",
                nameof(companyId));


        return new CompanyTicketCounter
        {
            CompanyId = companyId,
            LastTicketNumber = 0
        };
    }


    public long GetNextNumber()
    {
        LastTicketNumber++;

        return LastTicketNumber;
    }
}