using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace MultiMart.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure(IServiceProvider serviceProvider)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IRegister)) && t is { IsInterface: false, IsAbstract: false });
        foreach (var type in types)
        {
            var register = (IRegister)ActivatorUtilities.CreateInstance(serviceProvider, type);
            register.Register(config);
        }
    }
}