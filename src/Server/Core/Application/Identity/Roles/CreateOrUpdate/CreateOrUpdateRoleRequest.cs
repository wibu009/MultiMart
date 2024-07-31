namespace MultiMart.Application.Identity.Roles.CreateOrUpdate;

public class CreateOrUpdateRoleRequest : IRequest<string>
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}