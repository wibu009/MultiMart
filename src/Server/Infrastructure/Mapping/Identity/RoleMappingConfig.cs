using Mapster;
using MultiMart.Application.Identity.Roles;
using MultiMart.Infrastructure.Identity.Role;
using MultiMart.Shared.Authorization;

namespace MultiMart.Infrastructure.Mapping.Identity;

public class RoleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationRole, RoleDto>()
            .Map(dest => dest.Permissions, src => src.RoleClaims
                .Where(c => c.ClaimType == ApplicationClaims.Permission)
                .Select(c => c.ClaimValue)
                .ToList());
    }
}