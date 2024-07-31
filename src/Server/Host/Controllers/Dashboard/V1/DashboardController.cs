using MultiMart.Application.Dashboard.V1;

namespace MultiMart.Host.Controllers.Dashboard.V1;

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