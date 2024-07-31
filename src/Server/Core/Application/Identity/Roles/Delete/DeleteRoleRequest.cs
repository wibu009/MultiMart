namespace MultiMart.Application.Identity.Roles.Delete;

public class DeleteRoleRequest : IRequest<string>
{
    public string Id { get; set; }

    public DeleteRoleRequest(string id)
    {
        Id = id;
    }
}