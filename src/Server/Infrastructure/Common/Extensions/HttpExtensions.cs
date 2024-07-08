using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace MultiMart.Infrastructure.Common.Extensions;

public static class HttpExtensions
{
    public static string GetIpAddress(this HttpContext context)
    {
        string? ip = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        return ip ?? "N/A";
    }

    public static string GetIpAddress(this HttpRequest request)
    {
        string? ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

        return ip ?? "N/A";
    }

    public static string GetUri(this HttpRequest request)
    {
        return request.Headers.TryGetValue("Referer", out var header) ? header.ToString() : string.Empty;
    }

    public static string GetOrigin(this HttpRequest request)
    {
        string scheme = request.Scheme;
        var host = request.Host;
        var pathBase = request.PathBase;

        return $"{scheme}://{host}{pathBase}";
    }
}