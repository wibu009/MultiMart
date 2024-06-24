using MultiMart.Application.Dashboard;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    [HttpGet]
    [RequiresPermission(ApplicationAction.View, ApplicationResource.Dashboard)]
    [SwaggerOperation("Get statistics for the dashboard.", "")]
    public Task<StatsDto> GetAsync()
    {
        return Mediator.Send(new GetStatsRequest());
    }
}