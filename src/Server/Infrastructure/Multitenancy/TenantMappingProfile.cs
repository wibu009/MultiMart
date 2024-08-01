using Mapster;
using MultiMart.Application.Common.Persistence;
using MultiMart.Application.Multitenancy;

namespace MultiMart.Infrastructure.Multitenancy;

public class TenantMappingProfile : IRegister
{
    private readonly IConnectionStringSecurer _connectionStringSecurer;
    public TenantMappingProfile(IConnectionStringSecurer connectionStringSecurer)
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