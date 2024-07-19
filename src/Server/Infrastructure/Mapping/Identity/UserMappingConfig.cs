using Mapster;
using MultiMart.Application.Identity.Users.Requests.Commands;
using MultiMart.Infrastructure.Identity.User;

namespace MultiMart.Infrastructure.Mapping.Identity;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .IgnoreNullValues(true);
        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PhoneNumber ?? string.Empty)
            .Ignore(dest => dest.Email ?? string.Empty)
            .IgnoreNullValues(true);
    }
}