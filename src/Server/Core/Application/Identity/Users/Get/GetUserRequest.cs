namespace MultiMart.Application.Identity.Users.Get;

public class GetUserRequest : IRequest<UserDetailsDto>
{
    public string Id { get; set; }

    public GetUserRequest(string id)
    {
        Id = id;
    }
}