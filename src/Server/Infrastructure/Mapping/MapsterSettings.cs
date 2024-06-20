using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace MultiMart.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure(IServiceProvider serviceProvider)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // Get all currently loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IRegister)) && t is { IsInterface: false, IsAbstract: false });
            foreach (var type in types)
            {
                var register = (IRegister)ActivatorUtilities.CreateInstance(serviceProvider, type);
                register.Register(config);
            }
        }
    }
}