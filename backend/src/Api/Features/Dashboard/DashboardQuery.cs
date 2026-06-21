using MediatR;

namespace Api.Features.Dashboard;

public sealed record DashboardQuery(Guid CompanyId)
: IRequest<DashboardResponse>;

