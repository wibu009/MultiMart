using Mapster;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Multitenancy;
using MultiMart.Infrastructure.Multitenancy;

namespace MultiMart.Infrastructure.Mapping.Multitenancy;

public class TenantMappingConfig : IRegister
{
    private readonly IConnectionStringSecurer _connectionStringSecurer;
    public TenantMappingConfig(IConnectionStringSecurer connectionStringSecurer)
    {
        _connectionStringSecurer = connectionStringSecurer;
    }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationTenantInfo, TenantDto>()
            .Map(
                dest => dest.ConnectionString,
                src => _connectionStringSecurer.MakeSecure(src.ConnectionString, src.DbProvider));
    }
}