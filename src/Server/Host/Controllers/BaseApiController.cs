using MediatR;
using MultiMart.Application.Common.Models;

namespace MultiMart.Host.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleRedirect(string url)
    {
        return Request.Headers["Referer"].ToString().Contains("/swagger")
            ? Ok(url)
            : Redirect(url);
    }
}