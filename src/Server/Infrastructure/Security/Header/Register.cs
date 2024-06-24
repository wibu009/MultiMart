using Microsoft.AspNetCore.Builder;

namespace MultiMart.Infrastructure.Security.Header;

internal static class Register
{
    internal static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(opt => opt.NoReferrer());
        app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
        app.UseXfo(opt => opt.Deny());
        app.UseCsp(opt => opt
            .BlockAllMixedContent()
            .StyleSources(s => s.Self().UnsafeInline())
            .FontSources(s => s.Self())
            .FormActions(s => s.Self())
            .FrameAncestors(s => s.Self())
            .ImageSources(s => s.Self())
            .ScriptSources(s => s.Self().UnsafeInline()));
        app.UseHttpsRedirection();
        app.UseHsts();
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
            await next.Invoke();
        });
        return app;
    }
}