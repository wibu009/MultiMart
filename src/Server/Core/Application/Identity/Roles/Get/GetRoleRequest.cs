namespace MultiMart.Application.Identity.Roles.Get;

public class GetRoleRequest : IRequest<RoleDto>
{
    public string Id { get; set; }

    public GetRoleRequest(string id)
    {
        Id = id;
    }
}