using System.Reflection;
using Mapster;

namespace MultiMart.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        var config = TypeAdapterConfig.GlobalSettings;

        // Get all currently loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IRegister)) && t is { IsInterface: false, IsAbstract: false });
            foreach (var type in types)
            {
                var register = (IRegister)Activator.CreateInstance(type)!;
                register.Register(config);
            }
        }
    }
}