using BookStack.Application.Dashboard;

namespace BookStack.Host.Controllers.Dashboard;

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