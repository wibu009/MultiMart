namespace MultiMart.Application.Identity.Users.GetUserPermissions;

public class GetUserPermissionsRequest : IRequest<List<string>>
{
    public string Id { get; set; }

    public GetUserPermissionsRequest(string id)
    {
        Id = id;
    }
}