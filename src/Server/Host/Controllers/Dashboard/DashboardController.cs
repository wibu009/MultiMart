using MultiMart.Application.Dashboard;

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