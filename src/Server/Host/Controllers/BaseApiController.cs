using MediatR;

namespace MultiMart.Host.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult RedirectIfNotSwagger(string url)
    {
        if (Request.Headers["Referer"].ToString().Contains("swagger", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(url);
        }

        return Redirect(url);
    }
}