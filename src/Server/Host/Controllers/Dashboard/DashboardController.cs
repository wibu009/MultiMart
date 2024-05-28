using MultiMart.Application.Dashboard;
using MultiMart.Infrastructure.Auth.Permissions;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    [HttpGet]
    [MustHavePermission(ApplicationAction.View, ApplicationResource.Dashboard)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<StatsDto> GetAsync()
    {
        return Mediator.Send(new GetStatsRequest());
    }
}