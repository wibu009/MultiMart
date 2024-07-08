using Microsoft.AspNetCore.Http;

namespace MultiMart.Infrastructure.Auth;

public class CurrentUserMiddleware : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer;

    public CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) =>
        _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser((context.User.Identity?.IsAuthenticated == true ? context.User : null)!);

        await next(context);
    }
}