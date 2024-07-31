namespace MultiMart.Application.Identity.Users.GetUserRole;

public class GetUserRoleRequest : IRequest<List<UserRoleDto>>
{
    public string Id { get; set; }

    public GetUserRoleRequest(string id)
    {
        Id = id;
    }
}