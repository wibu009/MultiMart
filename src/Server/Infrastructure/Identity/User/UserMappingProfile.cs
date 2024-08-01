using Mapster;
using MultiMart.Application.Identity.Users.Create;
using MultiMart.Application.Identity.Users.Update;

namespace MultiMart.Infrastructure.Identity.User;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .IgnoreNullValues(true);
        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PhoneNumber!)
            .Ignore(dest => dest.Email!)
            .IgnoreNullValues(true);
    }
}