using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace MultiMart.Infrastructure.Common.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage,
        int totalItems, int totalPages)
    {
        var paginationHeader = new
        {
            currentPage,
            itemsPerPage,
            totalItems,
            totalPages
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
    }

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

    public static string AddQueryParam(this string url, string key, string value)
    {
        if (url == null)
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key is null or empty", nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        string qs = $"{key}={Uri.EscapeDataString(value)}";

        return url.Contains('?') ? $"{url}&{qs}" : $"{url}?{qs}";
    }

    public static Dictionary<string, string> DecodeQueryStringToDict(this string queryString)
    {
        var parsed = HttpUtility.ParseQueryString(queryString);

        return parsed.AllKeys.ToDictionary(key => key, key => HttpUtility.UrlDecode(parsed[key]))!;
    }
}