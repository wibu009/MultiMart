using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MultiMart.Infrastructure.FileStorage.Cloudinary;

namespace MultiMart.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration config)
        => services.Configure<CloudinarySettings>(config.GetSection(nameof(CloudinarySettings)));
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app) =>
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            RequestPath = new PathString("/Files")
        });
}